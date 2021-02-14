#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
#include <DHT.h>
#include <ArduinoJson.h>

#define DHTPIN 2 // DHT11 data pin is connected to ESP8266 pin GPIO1 (TX)
#define DHTTYPE DHT22 // DHT 22 (AM2302)
#define MINIMUM_SAMPLING_PERIOD 2000

#ifndef STASSID
#define STASSID "STASSID"
#define STAPSK  "STAPSK"
#endif

#define APIKEY "SensorID"
#define SERVER_URL "YourServerUrl"

DHT dht(DHTPIN, DHTTYPE); // Configure DHT library

String macAddress;

const int capacity = JSON_OBJECT_SIZE(8);
StaticJsonDocument<capacity> doc;

void setup() {
  macAddress = WiFi.macAddress();
  
  Serial.begin(115200);
  Serial.println();
  Serial.println();
  Serial.print("MAC: ");
  Serial.println(macAddress);
  Serial.println();

  // https://github.com/esp8266/Arduino/issues/2186
  WiFi.persistent(false);
  WiFi.mode(WIFI_OFF);
  WiFi.mode(WIFI_STA);
  WiFi.begin(STASSID, STAPSK);

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("");
  Serial.print("Connected! IP address: ");
  Serial.println(WiFi.localIP());

  dht.begin();

  doc["SensorId"] = APIKEY;
  doc["MacAddress"] = macAddress;
}

void loop() {
  // wait for WiFi connection
  if ((WiFi.status() == WL_CONNECTED)) {

    Serial.println("Read sensor data.");

    // Read humidity
    float humidity = dht.readHumidity();

    //Read temperature in degree Celsius
    float temperature = dht.readTemperature();

    // Read temperature as Fahrenheit (isFahrenheit = true)
    float fahrenheitTemperature = dht.readTemperature(true);

    if (isnan(humidity) || isnan(temperature) || isnan(fahrenheitTemperature)) {
      Serial.println("Sensor returned NAN.");
      delay(MINIMUM_SAMPLING_PERIOD);
      return;
    }

    // prepare JSON data
    doc["Temperature"] = temperature;
    doc["Humidity"] = humidity;

    // Produce a minified JSON document
    Serial.println("Produce a minified JSON document: ");
    String jsonOutput;
    serializeJson(doc, jsonOutput);
    Serial.print(jsonOutput);
    
    Serial.println("[HTTP] begin...");

    WiFiClient client;
    HTTPClient http;
    
    // configure traged server and url
    http.begin(client, SERVER_URL);
    http.addHeader("Content-Type", "application/json");
    
    // start connection and send HTTP header and body
    Serial.println("[HTTP] POST...");
    int httpCode = http.POST(jsonOutput);

    // httpCode will be negative on error
    if (httpCode > 0) {
      // HTTP header has been send and Server response header has been handled
      Serial.printf("[HTTP] POST... code: %d\n", httpCode);

      // file found at server
      if (httpCode == HTTP_CODE_OK) {
        const String& payload = http.getString();
        Serial.println("received payload:\n<<");
        Serial.println(payload);
        Serial.println(">>");
      }
    } else {
      Serial.printf("[HTTP] POST... failed, error: %s\n", http.errorToString(httpCode).c_str());
    }

    http.end();
  }

  delay(10000);
}

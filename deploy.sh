dotnet publish -c Release
sudo cp -R bin/Release/net5.0/publish/* /var/www/TempStation/
sudo chown www-data:www-data -R /var/www/TempStation
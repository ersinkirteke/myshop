# myshop
dockerized microservices 

* Tüm sistemi dockerize ettim. docker-compose.dcproj projesi de uygulama içerisinde yer almaktadır.
* İçlerine ileriye dönük mimari yapılacak şeyleri TODO olarak yazdım. Onlara bakarak mimari tarafını ileriye dönük nasıl tasarladığımı görebilirsiniz.
* Apigateway olarak ocelot kullandım. Aynı zamanda tüm apileri swagger ile dökümente ettim ve open api şekline yazdım.
* Apigateway üzerinden tüm downstream microservislere ait swaggerları apigateway üzerinden erişilebilir ve authenticate edilebilir yaptım.
* Identity.API ile login olunurken username:ersinkirteke ve password:12345 ile authenticate olunduktan sonra bearer token ile diğer apilere authenticate olarak api methodları çağrılmaktadır.
* Ayrıca shopping cart kısmını azure durable function ve azure durable entity kullanarak  function as a service olarak kodladım.
* Sadece testlerimin içini dolduramadım. Normalde tdd kodlarım ama yoğunluktan ilk testi yazıp sonra kodlamaya başlayamadım.Test kısımlarını da vakit bulursam yarın tamamlamayı düşünüyorum.
* Stock controllerini basket'e eklerken yaptım. Ama bunu web artı mobil için bff yazıp oradaki service layer'a business'ı taşımak isterdim. Ayrıca yaptığım iki ayrı call'u oradaki servisler üzerinden yapmak doğru olanı, ama proje çok daha fazla genişleyecek ve vakit dar diye yapamadım.


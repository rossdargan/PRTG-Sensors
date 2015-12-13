// The DHT library for the temp sensor (I used V 1.2.3 by adafruit)
#include <DHT.h>

#include <SPI.h>
#include <Ethernet.h>


// Data wire is plugged into port 7 on the Arduino
// Connect a 4.7K resistor between VCC and the data pin (strong pullup)
#define DHT22_PIN 7

// LDR Pin
const int ldrInPin = A1; 

// Reseviour level pin
const int switchPin = 8;

// value for the temp sensor
int rawLightValue = 0;        // value read from the pot

int outputLightValue = 0;        // value output to the PWM (analog out)

int switchValue = LOW;
// Setup a DHT22 instance
DHT  myDHT22(DHT22_PIN, DHT22);

// Enter a MAC address and IP address for your controller below.
// The IP address will be dependent on your local network:
byte mac[] = {
  0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xED
};

IPAddress ip(192, 168, 1, 44);

// Initialize the Ethernet server library
// with the IP address and port you want to use
// (port 80 is default for HTTP):
EthernetServer server(80);

void setup() {
  // Open serial communications and wait for port to open:
  Serial.begin(9600);
   
  // start the Ethernet connection and the server:
  Ethernet.begin(mac, ip);
  server.begin();
  Serial.print("server is at ");
  Serial.println(Ethernet.localIP());
}


void loop() {
  // listen for incoming clients
  EthernetClient client = server.available();
  if (client) {
     
    Serial.println("new client");
    // an http request ends with a blank line
    boolean currentLineIsBlank = true;
    while (client.connected()) {
      if (client.available()) {
        char c = client.read();
        Serial.write(c);
        // if you've gotten to the end of the line (received a newline
        // character) and the line is blank, the http request has ended,
        // so you can send a reply
        if (c == '\n' && currentLineIsBlank) {
          sendResponse(client);         
          break;
        }
        if (c == '\n') {
          // you're starting a new line
          currentLineIsBlank = true;
        } else if (c != '\r') {
          // you've gotten a character on the current line
          currentLineIsBlank = false;
        }
      }
    }
    // give the web browser time to receive the data
    delay(1);
    // close the connection:
    client.stop();
    Ethernet.maintain();
  }
}

void sendResponse(EthernetClient client)
{
   // send a standard http response header
  sendHeader(client);
  client.println("<prtg>");          
  sendLightData(client);          
  sendResevoirData(client);          
  sendTempData(client);
  client.println("</prtg>");
}
void sendHeader(EthernetClient client)
{
  client.println("HTTP/1.1 200 OK");
  client.println("Content-Type: application/xml");
  client.println("Connection: close");  // the connection will be closed after completion of the response          
  client.println();
  client.println("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
}

void sendLightData(EthernetClient client)
{
    // read the analog in value:
    rawLightValue = analogRead(ldrInPin);
        
    // map it to the range of the analog out:
    outputLightValue = map(rawLightValue, 0, 800, 0, 100);
    // Cap it at 100%
    if(outputLightValue >100)
    {
      outputLightValue = 100;
    }
    
    client.print("<result><channel>Light</channel><unit>Percent</unit><float>1</float><value>");
    client.print(outputLightValue);
    client.println("</value></result>");
}

void sendTempData(EthernetClient client)
{
    client.print("<result><channel>Temperature</channel><unit>Custom</unit><customUnit>°C</customUnit><float>1</float><value>");
    client.print(myDHT22.readTemperature());
    client.print("</value></result><result><channel>Humidity</channel><unit>Percent</unit><float>1</float><value>");
    client.print(myDHT22.readHumidity());
    client.println("</value></result>");
}
void sendResevoirData(EthernetClient client)
{
  switchValue = digitalRead(switchPin);
  client.print("<result><channel>Reservoir</channel><valueLookup>developingtrends.greenhouse.reservoir</valueLookup><value>");
  client.print(switchValue);
  client.println("</value></result>");
}

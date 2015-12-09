#include <DHT22.h>

/*
  Web Server

 A simple web server that shows the value of the analog input pins.
 using an Arduino Wiznet Ethernet shield.

 Circuit:
 * Ethernet shield attached to pins 10, 11, 12, 13
 * Analog inputs attached to pins A0 through A5 (optional)

 created 18 Dec 2009
 by David A. Mellis
 modified 9 Apr 2012
 by Tom Igoe
 modified 02 Sept 2015
 by Arturo Guadalupi

 */

#include <SPI.h>
#include <Ethernet.h>


// Data wire is plugged into port 7 on the Arduino
// Connect a 4.7K resistor between VCC and the data pin (strong pullup)
#define DHT22_PIN 7
const int analogInPin = A1;  // LDR Pin

const int switchPin = 8;

int sensorValue = 0;        // value read from the pot

int outputValue = 0;        // value output to the PWM (analog out)

int switchValue = LOW;
// Setup a DHT22 instance
DHT22 myDHT22(DHT22_PIN);

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
    // The sensor can only be read from every 1-2s, and requires a minimum
  // 2s warm-up after power-on.
  delay(2000);

  
  // listen for incoming clients
  EthernetClient client = server.available();
  if (client) {
     DHT22_ERROR_t errorCode;
     
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

            // send a standard http response header
            client.println("HTTP/1.1 200 OK");
            client.println("Content-Type: application/xml");
            client.println("Connection: close");  // the connection will be closed after completion of the response          
            client.println();
            client.println("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
            client.println("<prtg>");

            switchValue = digitalRead(switchPin);


            // read the analog in value:
            sensorValue = analogRead(analogInPin);

                      
            // map it to the range of the analog out:
            outputValue = map(sensorValue, 0, 800, 0, 100);
            if(outputValue >100)
            {
              outputValue = 100;
            }
            client.print("<result><channel>Light</channel><unit>Percent</unit><float>1</float><value>");
            client.print(outputValue);
            client.println("</value></result>");

               client.print("<result><channel>Reservoir</channel><valueLookup>developingtrends.greenhouse.reservoir</valueLookup><value>");
            client.print(switchValue);
            client.println("</value></result>");
            errorCode = myDHT22.readData();
            switch(errorCode)
            {
              case DHT_ERROR_NONE:
                client.print("<result><channel>Temperature</channel><unit>Custom</unit><customUnit>°C</customUnit><float>1</float><value>");
                client.print(myDHT22.getTemperatureC());
                client.print("</value></result><result><channel>Humidity</channel><unit>Percent</unit><float>1</float><value>");
                client.print(myDHT22.getHumidity());
                client.println("</value></result>");
                // Alternately, with integer formatting which is clumsier but more compact to store and
             // can be compared reliably for equality:
              //    
          //      char buf[128];
           //     sprintf(buf, "Integer-only reading: Temperature %hi.%01hi C, Humidity %i.%01i %% RH",
           //                  myDHT22.getTemperatureCInt()/10, abs(myDHT22.getTemperatureCInt()%10),
           //                  myDHT22.getHumidityInt()/10, myDHT22.getHumidityInt()%10);
            //    client.println(buf);
                break;
              case DHT_ERROR_CHECKSUM:
                client.print("<error>check sum error </error>");
                //client.print(myDHT22.getTemperatureC());
                //client.print("C ");
                //client.print(myDHT22.getHumidity());
                //client.println("%");
                break;
              case DHT_BUS_HUNG:
                client.println("<error>BUS Hung </error>");
                break;
              case DHT_ERROR_NOT_PRESENT:
                client.println("<error>Not Present </error>");
                break;
              case DHT_ERROR_ACK_TOO_LONG:
                client.println("<error>ACK time out </error>");
                break;
              case DHT_ERROR_SYNC_TIMEOUT:
                client.println("<error>Sync Timeout </error>");
                break;
              case DHT_ERROR_DATA_TIMEOUT:
                client.println("<error>Data Timeout </error>");
                break;
              case DHT_ERROR_TOOQUICK:
                client.println("<error>Polled to quick </error>");
                break;
            }

  

                
          client.println("</prtg>");
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


//This is the Bluetooth control with typed-in values for the brightness.
//Make sure the number of pixels is correct if using a different lighting rig.
//This is for a rig with 4 sides, with 24 pixels vertically and 36 pixels horizontally


#include <SPI.h>
#include <Adafruit_NeoPixel.h>
#ifdef __AVR__
#include <avr/power.h>
#endif
#include <Arduino.h>
#include <SPI.h>

#if SOFTWARE_SERIAL_AVAILABLE
#include <SoftwareSerial.h>
#endif


#define PIN 6

String stringIn = "";
int intIn = 0;
String sideSelected = "all";




Adafruit_NeoPixel strip = Adafruit_NeoPixel(120, PIN, NEO_GRB + NEO_KHZ800);


uint8_t current_state = 0;
int totalCoord[3] = {0, 119, 0};
int leftCoord[3] = {0, 23, 0};
int topCoord[3] = {24, 59, 0};
int rightCoord[3] = {60, 83, 0};
int bottomCoord[3] = {84, 119, 0};

void setup() {
  Serial.begin(9600);


  //Neopixel Initialization

  strip.begin();
  for (int i = 0; i > 120; i++) {
    strip.setPixelColor(i, strip.Color(0, 0, 0));
  }
  pinMode(PIN, OUTPUT);
  pinMode(8, INPUT);



}

void loop() {

  if (Serial.available() > 0) { // Checks whether data is comming from the serial port
    stringIn = Serial.readString(); // Reads the data from the serial port
    intIn = stringIn.toInt();
    //Serial.println(intIn);

    if (stringIn == "top" || stringIn == "bottom" || stringIn == "right" || stringIn == "left" || stringIn == "all") {
      sideSelected = stringIn;
      Serial.print("side selected: ");
      Serial.println(sideSelected);

    }
    if (stringIn == "0") {
      Serial.print("brightness selected: ");
      if (sideSelected == "top") {
        changeBrightness(intIn, topCoord);
      } else if (sideSelected == "bottom") {
        changeBrightness(intIn, bottomCoord);
      } else if (sideSelected == "left") {
        changeBrightness(intIn, leftCoord);
      } else if (sideSelected == "right") {
        changeBrightness(intIn, rightCoord);
      } else if (sideSelected == "all") {
        changeBrightness(intIn, totalCoord);
      }
    }

    if (intIn > 0) {
      Serial.print("brightness selected: ");
      if (sideSelected == "top") {
        changeBrightness(intIn, topCoord);
      } else if (sideSelected == "bottom") {
        changeBrightness(intIn, bottomCoord);
      } else if (sideSelected == "left") {
        changeBrightness(intIn, leftCoord);
      } else if (sideSelected == "right") {
        changeBrightness(intIn, rightCoord);
      } else if (sideSelected == "all") {
        changeBrightness(intIn, totalCoord);
      }

    }

  }
  //Serial.print(intIn);
  strip.show();


}

void changeBrightness(int increment, int side[3]) { //sign - 1 or -1, indicates increase or decrease
  int currentBrightness = side[2];
  int newBrightness = increment;
  if (newBrightness <= 255 ) {
    newBrightness = increment;
    for (int i = side[0]; i <= side[1]; i++) {
      //Serial.println(i);
      strip.setPixelColor(i, strip.Color(newBrightness, newBrightness, newBrightness));
    }
  }


  side[2] = newBrightness;
  Serial.println(newBrightness);
}


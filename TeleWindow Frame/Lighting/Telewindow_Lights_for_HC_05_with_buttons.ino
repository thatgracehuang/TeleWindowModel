//This is the version of Bluetooth control with button presses.
//Each press increases brightness by 50
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

String state = "";





Adafruit_NeoPixel strip = Adafruit_NeoPixel(120, PIN, NEO_GRB + NEO_KHZ800);


uint8_t current_state = 0;
bool pressed = false;
int increment = 50;

int totalCoord[3] = {0, 120, 0};
int leftCoord[3] = {0, 23, 0};
int rightCoord[3] = {24, 59, 0};
int topCoord[3] = {60, 83, 0};
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
    state = Serial.readString(); // Reads the data from the serial port
    Serial.println(state);

    if (state == "top") {
      Serial.println(state);

      changeBrightness(increment, topCoord, 1);

    }
  }
  else if (state == "right") {
    Serial.println(state);
    changeBrightness(increment, rightCoord, 1);


  }

  else if (state == "left") {
    Serial.println(state);
    changeBrightness(increment, leftCoord, 1);

  }  else if (state == "bottom") {
    Serial.println(state);
    changeBrightness(increment, bottomCoord, 1);

  }
  else if (state == "all") {
    Serial.println(state);
    changeBrightness(increment, topCoord, 1);
    changeBrightness(increment, bottomCoord, 1);
    changeBrightness(increment, leftCoord, 1);
    changeBrightness(increment, rightCoord, 1);




  }




  strip.show();



}

void changeBrightness(int increment, int side[3], int sign) { //sign - 1 or -1, indicates increase or decrease
  Serial.print("into the function");
  int currentBrightness = side[2];
  int newBrightness = currentBrightness;
  if (currentBrightness < 250 ) {
    newBrightness = currentBrightness + increment;
  } else if (currentBrightness == 250) {
    newBrightness = 0;
  }
  
    for (int i = side[0]; i <= side[1]; i++) {
    strip.setPixelColor(i, strip.Color(newBrightness, newBrightness, newBrightness));
    }
  side[2] = newBrightness;
  Serial.println(side[2]);
  state = "";
}


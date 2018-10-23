EESchema Schematic File Version 4
EELAYER 26 0
EELAYER END
$Descr A4 11693 8268
encoding utf-8
Sheet 1 1
Title ""
Date ""
Rev ""
Comp ""
Comment1 ""
Comment2 ""
Comment3 ""
Comment4 ""
$EndDescr
$Comp
L Connector:Raspberry_Pi_2_3 RPi
U 1 1 5BCF6E5B
P 2300 2600
F 0 "RPi" H 2300 4078 50  0000 C CNN
F 1 "Raspberry Pi 3" H 2300 3987 50  0000 C CNN
F 2 "" H 2300 2600 50  0001 C CNN
F 3 "https://www.raspberrypi.org/documentation/hardware/raspberrypi/schematics/rpi_SCH_3bplus_1p0_reduced.pdf" H 2300 2600 50  0001 C CNN
	1    2300 2600
	1    0    0    -1  
$EndComp
$Comp
L RF:NRF24L01_Breakout NRF1
U 1 1 5BCF70FE
P 5350 1700
F 0 "NRF1" H 5828 1678 50  0000 L CNN
F 1 "nRF24L01+" H 5828 1587 50  0000 L CNN
F 2 "RF_Module:nRF24L01_Breakout" H 5500 2300 50  0001 L CIN
F 3 "http://www.nordicsemi.com/eng/content/download/2730/34105/file/nRF24L01_Product_Specification_v2_0.pdf" H 5350 1600 50  0001 C CNN
	1    5350 1700
	1    0    0    -1  
$EndComp
$Comp
L power:GND #PWR?
U 1 1 5BCF714C
P 5350 2350
F 0 "#PWR?" H 5350 2100 50  0001 C CNN
F 1 "GND" H 5355 2177 50  0000 C CNN
F 2 "" H 5350 2350 50  0001 C CNN
F 3 "" H 5350 2350 50  0001 C CNN
	1    5350 2350
	1    0    0    -1  
$EndComp
Wire Wire Line
	5350 2350 5350 2300
$Comp
L power:+3V3 #PWR?
U 1 1 5BCF720C
P 5350 1050
F 0 "#PWR?" H 5350 900 50  0001 C CNN
F 1 "+3V3" H 5365 1223 50  0000 C CNN
F 2 "" H 5350 1050 50  0001 C CNN
F 3 "" H 5350 1050 50  0001 C CNN
	1    5350 1050
	1    0    0    -1  
$EndComp
Wire Wire Line
	5350 1050 5350 1100
Wire Wire Line
	3100 2900 3700 2900
Wire Wire Line
	4000 2900 4000 1500
Wire Wire Line
	4000 1500 4850 1500
Wire Wire Line
	3100 3000 3800 3000
Wire Wire Line
	4100 3000 4100 1400
Wire Wire Line
	4100 1400 4850 1400
Wire Wire Line
	3100 3100 3600 3100
Wire Wire Line
	4200 3100 4200 1600
Wire Wire Line
	4200 1600 4850 1600
Wire Wire Line
	3100 2800 3900 2800
Wire Wire Line
	3900 2800 3900 1700
Wire Wire Line
	3900 1700 4850 1700
Wire Wire Line
	4850 1900 3800 1900
Wire Wire Line
	3800 1900 3800 2500
Wire Wire Line
	3800 2500 3100 2500
Wire Wire Line
	4850 2000 3700 2000
Wire Wire Line
	3700 2000 3700 2400
Wire Wire Line
	3700 2400 3100 2400
$Comp
L RF:NRF24L01_Breakout NRF2
U 1 1 5BCF812D
P 5350 3800
F 0 "NRF2" H 5828 3778 50  0000 L CNN
F 1 "nRF24L01+" H 5828 3687 50  0000 L CNN
F 2 "RF_Module:nRF24L01_Breakout" H 5500 4400 50  0001 L CIN
F 3 "http://www.nordicsemi.com/eng/content/download/2730/34105/file/nRF24L01_Product_Specification_v2_0.pdf" H 5350 3700 50  0001 C CNN
	1    5350 3800
	1    0    0    -1  
$EndComp
$Comp
L power:+3V3 #PWR?
U 1 1 5BCF81A2
P 5350 3150
F 0 "#PWR?" H 5350 3000 50  0001 C CNN
F 1 "+3V3" H 5365 3323 50  0000 C CNN
F 2 "" H 5350 3150 50  0001 C CNN
F 3 "" H 5350 3150 50  0001 C CNN
	1    5350 3150
	1    0    0    -1  
$EndComp
$Comp
L power:GND #PWR?
U 1 1 5BCF81B3
P 5350 4500
F 0 "#PWR?" H 5350 4250 50  0001 C CNN
F 1 "GND" H 5355 4327 50  0000 C CNN
F 2 "" H 5350 4500 50  0001 C CNN
F 3 "" H 5350 4500 50  0001 C CNN
	1    5350 4500
	1    0    0    -1  
$EndComp
Wire Wire Line
	5350 3150 5350 3200
Wire Wire Line
	5350 4400 5350 4500
Wire Wire Line
	4850 3500 3800 3500
Wire Wire Line
	3800 3500 3800 3000
Connection ~ 3800 3000
Wire Wire Line
	3800 3000 4100 3000
Wire Wire Line
	4850 3600 3700 3600
Wire Wire Line
	3700 3600 3700 2900
Connection ~ 3700 2900
Wire Wire Line
	3700 2900 4000 2900
Wire Wire Line
	4850 3700 3600 3700
Wire Wire Line
	3600 3700 3600 3100
Connection ~ 3600 3100
Wire Wire Line
	3600 3100 4200 3100
Wire Wire Line
	4850 3800 3500 3800
Wire Wire Line
	3500 3800 3500 2700
Wire Wire Line
	3500 2700 3100 2700
Wire Wire Line
	4850 4000 1200 4000
Wire Wire Line
	1200 4000 1200 3200
Wire Wire Line
	1200 3200 1500 3200
Wire Wire Line
	4850 4100 1300 4100
Wire Wire Line
	1300 4100 1300 3300
Wire Wire Line
	1300 3300 1500 3300
$EndSCHEMATC

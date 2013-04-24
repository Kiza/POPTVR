clear all
clc

%% lebal 1

centriod = 0.16692963
;
width = 0.001397367
;
s = 100* width;

x = -s+centriod : width* 10^-1 : s+centriod;
y = exp( - (x - centriod).^2 / width);

%% lebal 2

centriod = 0.222824316
;
width = 0.000825435
;
s = 100* width;

x2 = -s+centriod : width* 10^-1 : s+centriod;
y2 = exp( - (x2 - centriod).^2 / width);

%% lebal 3

centriod = 0.25584172
;
width = 0.000825435
;
s = 100* width;

x3 = -s+centriod : width* 10^-1 : s+centriod;
y3 = exp( - (x3 - centriod).^2 / width);

%% lebal 4
centriod = 0.292091959
;
width = 0.000906256
;
s = 100* width;

x4 = -s+centriod : width* 10^-1 : s+centriod;
y4 = exp( - (x4 - centriod).^2 / width);

%% lebal 5
centriod = 0.371597859
;
width = 0.001175804
;
s = 100* width;

x5 = -s+centriod : width* 10^-1 : s+centriod;
y5 = exp( - (x5 - centriod).^2 / width);

%% lebal 6
centriod = 0.418630033
;
width = 0.000751946
;
s = 100* width;

x6 = -s+centriod : width* 10^-1 : s+centriod;
y6 = exp( - (x6 - centriod).^2 / width);

%% lebal 7
centriod = 0.448707874
;
width = 0.000751946
;
s = 200* width;

x7 = -s+centriod : width* 10^-1 : s+centriod;
y7 = exp( - (x7 - centriod).^2 / width);

%% lebal  8
centriod = 0.549442036
;
width = 0.000112283
;
s = 300* width;

x8 = -s+centriod : width* 10^-1 : s+centriod;
y8 = exp( - (x8 - centriod).^2 / width);

%% lebal  9
centriod = 0.553933359
;
width = 0.000112283
;
s = 300* width;

x9 = -s+centriod : width* 10^-1 : s+centriod;
y9 = exp( - (x9 - centriod).^2 / width);

%% lebal  10
centriod = 0.657753951
;
width = 0.000229007
;
s = 1000* width;

x10 = -s+centriod : width* 10^-1 : s+centriod;
y10 = exp( - (x10 - centriod).^2 / width);

%% lebal  11
centriod = 0.666914226
;
width = 0.000229007
;
s = 1000* width;

x11 = -s+centriod : width* 10^-1 : s+centriod;
y11 = exp( - (x11 - centriod).^2 / width);

%% lebal  12
centriod = 0.777571142
;
width = 4.73E-06
;
s = 100* width;

x12 = -s+centriod : width* 10^-1 : s+centriod;
y12 = exp( - (x12 - centriod).^2 / width);

%% lebal  13
centriod = 0.777760165
;
width = 4.73E-06
;
s = 2000* width;

x13 = -s+centriod : width* 10^-1 : s+centriod;
y13 = exp( - (x13 - centriod).^2 / width);

%% lebal  14
centriod = 0.831698454
;
width = 0.001275047
;
s = 100* width;

x14 = -s+centriod : width* 10^-1 : s+centriod;
y14 = exp( - (x14 - centriod).^2 / width);

%% lebal  15
centriod = 0.882700353
;
width = 0.001269991
;
s = 100* width;

x15 = -s+centriod : width* 10^-1 : s+centriod;
y15 = exp( - (x15 - centriod).^2 / width);

%% lebal  16
centriod = 0.9335
;
width = 0.001269991
;
s = 100* width;

x16 = -s+centriod : width* 10^-1 : s+centriod;
y16 = exp( - (x16 - centriod).^2 / width);

%%
plot(x,y,x2,y2, x3, y3, x4, y4, x5, y5, x6, y6, x7, y7, x8, y8, x9, y9,x10, y10,x11, y11,x12, y12,x13, y13,x14, y14,x15, y15,x16, y16)
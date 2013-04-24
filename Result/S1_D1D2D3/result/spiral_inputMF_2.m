clear all
clc

%% lebal 1

centriod = 4.954055198

width = 0.002571866

s = 100* width;

x = -s+centriod : width* 10^-1 : s+centriod;
y = exp( - (x - centriod).^2 / width);

%% lebal 2

centriod = 5.056929844

width = 0.001969611

s = 100* width;

x2 = -s+centriod : width* 10^-1 : s+centriod;
y2 = exp( - (x2 - centriod).^2 / width);

%% lebal 3

centriod = 5.135714286

width = 0.000185977

s = 1000* width;

x3 = -s+centriod : width* 10^-1 : s+centriod;
y3 = exp( - (x3 - centriod).^2 / width);

%% lebal 4
centriod = 5.143153354

width = 0.000185977

s = 1000* width;

x4 = -s+centriod : width* 10^-1 : s+centriod;
y4 = exp( - (x4 - centriod).^2 / width);

%% lebal 5
centriod = 5.153792325

width = 0.000265974

s = 1000* width;

x5 = -s+centriod : width* 10^-1 : s+centriod;
y5 = exp( - (x5 - centriod).^2 / width);

%% lebal 6
centriod = 5.181711477

width = 0.000697979

s = 100* width;

x6 = -s+centriod : width* 10^-1 : s+centriod;
y6 = exp( - (x6 - centriod).^2 / width);

%% lebal 7
centriod = 5.221428571

width = 0.000992927

s = 200* width;

x7 = -s+centriod : width* 10^-1 : s+centriod;
y7 = exp( - (x7 - centriod).^2 / width);



%%
plot(x,y,x2,y2, x3, y3, x4, y4, x5, y5, x6, y6, x7, y7)
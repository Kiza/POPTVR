clear all
clc

%% lebal 1

centriod = 0;
width = 0.00001;
s = 1000* width;

x = -s+centriod : width* 10^-1 : s+centriod;
y = exp( - (x - centriod).^2 / width);

%% lebal 2

centriod = 0;
width = 0.00001;
s = 1000* width;

x2 = -s+centriod : width* 10^-1 : s+centriod;
y2 = exp( - (x2 - centriod).^2 / width);

%% lebal 3

centriod = 1;
width = 0.00001;
s = 1000* width;

x3 = -s+centriod : width* 10^-1 : s+centriod;
y3 = exp( - (x3 - centriod).^2 / width);


%%
plot(x,y,x2,y2, x3, y3)
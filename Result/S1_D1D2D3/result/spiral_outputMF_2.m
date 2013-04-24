clear all
clc

%% lebal 1

centriod = 0.145833809
;
width = 0.021353362
;
s = 50* width;

x = -s+centriod : width* 10^-1 : s+centriod;
y = exp( - (x - centriod).^2 / width);

%% lebal 2

centriod = 0.999968284
;
width = 0.00416746
;
s = 100* width;

x2 = -s+centriod : width* 10^-1 : s+centriod;
y2 = exp( - (x2 - centriod).^2 / width);

%% lebal 3

centriod = 1.166666667
;
width = 0.00416746
;
s = 100* width;

x3 = -s+centriod : width* 10^-1 : s+centriod;
y3 = exp( - (x3 - centriod).^2 / width);


%%
plot(x,y,x2,y2, x3, y3)
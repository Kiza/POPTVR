clear all
clc

%% lebal 1

centriod = 0.002100746
;
width = 0.021822481
;
s = 50* width;

x = -s+centriod : width* 10^-1 : s+centriod;
y = exp( - (x - centriod).^2 / width);

%% lebal 2

centriod = 0.875
;
width = 0.003072914
;
s = 100* width;

x2 = -s+centriod : width* 10^-1 : s+centriod;
y2 = exp( - (x2 - centriod).^2 / width);

%% lebal 3

centriod = 0.997916546
;
width = 0.003072914
;
s = 100* width;

x3 = -s+centriod : width* 10^-1 : s+centriod;
y3 = exp( - (x3 - centriod).^2 / width);

%% lebal 4
centriod = 1.125
;
width = 0.003177086
;
s = 100* width;

x4 = -s+centriod : width* 10^-1 : s+centriod;
y4 = exp( - (x4 - centriod).^2 / width);


%%
plot(x,y,x2,y2, x3, y3, x4, y4)
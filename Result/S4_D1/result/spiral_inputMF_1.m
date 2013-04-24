clear all
clc

%% lebal 1

centriod = 28.00087646
;
width = 0.001763473
;
s = 100* width;

x = -s+centriod : width* 10^-1 : s+centriod;
y = exp( - (x - centriod).^2 / width);

%% lebal 2

centriod = 28.07141539
;
width = 0.001763473
;
s = 100* width;

x2 = -s+centriod : width* 10^-1 : s+centriod;
y2 = exp( - (x2 - centriod).^2 / width);

%% lebal 3

centriod = 28.88429064
;
width = 0.010035591
;
s = 100* width;

x3 = -s+centriod : width* 10^-1 : s+centriod;
y3 = exp( - (x3 - centriod).^2 / width);

%% lebal 4
centriod = 29.28571429
;
width = 0.007142857
;
s = 100* width;

x4 = -s+centriod : width* 10^-1 : s+centriod;
y4 = exp( - (x4 - centriod).^2 / width);

%% lebal 5
centriod = 29.57142857
;
width = 0.007142857
;
s = 100* width;

x5 = -s+centriod : width* 10^-1 : s+centriod;
y5 = exp( - (x5 - centriod).^2 / width);

%% lebal 6
centriod = 29.95150307
;
width = 0.002184467
;
s = 100* width;

x6 = -s+centriod : width* 10^-1 : s+centriod;
y6 = exp( - (x6 - centriod).^2 / width);

%% lebal 7
centriod = 30.03888176
;
width = 0.002184467
;
s = 200* width;

x7 = -s+centriod : width* 10^-1 : s+centriod;
y7 = exp( - (x7 - centriod).^2 / width);



%%
plot(x,y,x2,y2, x3, y3, x4, y4, x5, y5, x6, y6, x7, y7)
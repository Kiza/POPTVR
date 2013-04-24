clear all
clc

%% lebal 1

centriod = 0.199940937;
width = 0.00131795;
s = 100* width;

x = -s+centriod : width* 10^-1 : s+centriod;
y = exp( - (x - centriod).^2 / width);

%% lebal 2

centriod = 0.25265893;
width = 0.000485029;
s = 100* width;

x2 = -s+centriod : width* 10^-1 : s+centriod;
y2 = exp( - (x2 - centriod).^2 / width);

%% lebal 3

centriod = 0.272060084;
width = 0.000485029;
s = 100* width;

x3 = -s+centriod : width* 10^-1 : s+centriod;
y3 = exp( - (x3 - centriod).^2 / width);

%% lebal 4
centriod = 0.356493241;
width = 0.000891013;
s = 100* width;

x4 = -s+centriod : width* 10^-1 : s+centriod;
y4 = exp( - (x4 - centriod).^2 / width);

%% lebal 5
centriod = 0.392133767;
width = 0.000634511;
s = 100* width;

x5 = -s+centriod : width* 10^-1 : s+centriod;
y5 = exp( - (x5 - centriod).^2 / width);

%% lebal 6
centriod = 0.417514218;
width = 0.000634511;
s = 100* width;

x6 = -s+centriod : width* 10^-1 : s+centriod;
y6 = exp( - (x6 - centriod).^2 / width);

%% lebal 7
centriod = 0.468010205;
width = 0.000292637;
s = 200* width;

x7 = -s+centriod : width* 10^-1 : s+centriod;
y7 = exp( - (x7 - centriod).^2 / width);

%% lebal  8
centriod = 0.479715681;
width = 0.000292637;
s = 300* width;

x8 = -s+centriod : width* 10^-1 : s+centriod;
y8 = exp( - (x8 - centriod).^2 / width);

%% lebal  9
centriod = 0.585593814;
width = 0.000552911;
s = 300* width;

x9 = -s+centriod : width* 10^-1 : s+centriod;
y9 = exp( - (x9 - centriod).^2 / width);

%% lebal  10
centriod = 0.607710273;
width = 0.000552911;
s = 100* width;

x10 = -s+centriod : width* 10^-1 : s+centriod;
y10 = exp( - (x10 - centriod).^2 / width);

%% lebal  11
centriod = 0.639249747;
width = 0.000788487;
s = 100* width;

x11 = -s+centriod : width* 10^-1 : s+centriod;
y11 = exp( - (x11 - centriod).^2 / width);

%% lebal  12
centriod = 0.687108985;
width = 0.001196481;
s = 100* width;

x12 = -s+centriod : width* 10^-1 : s+centriod;
y12 = exp( - (x12 - centriod).^2 / width);

%% lebal  13
centriod = 0.755563161;
width = 0.001248977;
s = 200* width;

x13 = -s+centriod : width* 10^-1 : s+centriod;
y13 = exp( - (x13 - centriod).^2 / width);

%% lebal  14
centriod = 0.805522229;
width = 0.001117205;
s = 100* width;

x14 = -s+centriod : width* 10^-1 : s+centriod;
y14 = exp( - (x14 - centriod).^2 / width);

%% lebal  15
centriod = 0.850210422;
width = 0.001117205;
s = 100* width;

x15 = -s+centriod : width* 10^-1 : s+centriod;
y15 = exp( - (x15 - centriod).^2 / width);

%% lebal  16
centriod = 0.9005625;
width = 0.001258802;
s = 100* width;

x16 = -s+centriod : width* 10^-1 : s+centriod;
y16 = exp( - (x16 - centriod).^2 / width);

%%
plot(x,y,x2,y2, x3, y3, x4, y4, x5, y5, x6, y6, x7, y7, x8, y8, x9, y9,x10, y10,x11, y11,x12, y12,x13, y13,x14, y14,x15, y15,x16, y16)
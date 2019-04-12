% generateMorphingSequence("target.png","source.png", 8, 0.1);

% Receive string of img paths and generates N number of intermediates frames
%%
function generateMorphingSequence(sourceImgPath, targetImgPath, Nframes, delay)

% Load the image
sourceImg = double(imread(sourceImgPath));
targetImg = double(imread(targetImgPath));

gifName = "Images\MorphResult_"+extractBetween(sourceImgPath,8,9)+".gif";

% Perform DCT transform
F_u_v_s = dct2RGB(sourceImg);
F_u_v_t = dct2RGB(targetImg);

% Creating Transfer function
[M,N] = size(sourceImg);
N = N / 3; % N needed to be divided by 3 as there are 3 color channels

% Set up range of variables.
u = 0:(M - 1);
v = 0:(N - 1);

% Compute the indices for use in meshgrid.
idx = find(u > M/2);
u(idx) = u(idx) - M;
idy = find(v > N/2);
v(idy) = v(idy) - N;

% Compute the meshgrid arrays.
[V, U] = meshgrid(v, u);

% Compute the distances D(U, V).
D = sqrt(U.^2 + V.^2);

% Create the morphing sequence
for i=1:Nframes
    % Compute the cut-off frequency
    D0 = i / Nframes * sqrt(double((M-1).^2 + (N-1).^2));
    
    % Gaussian Filter
    Hlp = ifftshift(exp(-(D.^2)./(2*(D0.^2))));
    
    % Applying Gaussian Filter and IDCT    
    f = real(idct2RGB(Hlp.*(F_u_v_s - F_u_v_t) + F_u_v_t));

    % Write to the GIF File 
    [f_ind, c_map]= rgb2ind(uint8(f),256);
    if i==1
        imwrite(f_ind ,c_map,gifName,'gif','LoopCount',0)
    else
        imwrite(f_ind ,c_map,gifName,'gif','WriteMode','append','DelayTime',delay)
    end
end

% Write the last image to be the
[f_ind, c_map]= rgb2ind(imread(sourceImgPath),256);
imwrite(f_ind ,c_map,gifName,'gif','WriteMode','append','DelayTime',delay)
end

% 2-D DCT functions for RGB images
function output=dct2RGB(input)
output(:,:,1)=dct2(input(:,:,1));
output(:,:,2)=dct2(input(:,:,2));
output(:,:,3)=dct2(input(:,:,3));
end

% 2-D iDCT functions for RGB images
function output=idct2RGB(input)
output(:,:,1)=idct2(input(:,:,1));
output(:,:,2)=idct2(input(:,:,2));
output(:,:,3)=idct2(input(:,:,3));
end
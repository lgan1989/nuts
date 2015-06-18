import pygame
import sys

pygame.init()
screen = pygame.display.set_mode((100,100))

names = [
        'jungle_high_48.bmp',
        'jungle_high_64.bmp',
        'jungle_low_48.bmp',
        'jungle_low_64.bmp'
        ]

for file_name in names:

    surf = pygame.image.load(file_name).convert_alpha()

    pixel_array = pygame.PixelArray(  surf  )


    for i in range(len(pixel_array)):
        for j in range(len(pixel_array[0])):
            color = surf.unmap_rgb( pixel_array[i][j] )
            r , g , b, a= color[0] , color[1] , color[2] , color[3]
            #print r , g, b, a
            if (r,g,b) == (247,0,255):
                pixel_array[i][j] = pygame.Color(255,255,255,255)
            else:
                pixel_array[i][j] = pygame.Color(247,0,255,0)



    pygame.image.save(pixel_array.make_surface() , file_name.replace('.bmp' , '.png'))


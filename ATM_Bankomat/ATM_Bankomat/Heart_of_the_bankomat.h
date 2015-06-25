#pragma once
/* --------------------------------------------------------------------------------------*/
//									Підключаємо бібліотеки.								   //
/* --------------------------------------------------------------------------------------*/
#include <string>
#include <fstream>
#include <iostream>
#include <windows.h>
// Підключаємо header файли SDL бібліотек.
#include "Headers_SDL.h"
// Підключаємо header файли OpenGL бібліотеки.
#include "Headers_OpenGL.h"
#include "Defines.h"
#include "initialization_card.h"
typedef unsigned int uint;
class Heart_of_the_bankomat
{
private:
	/* --------------------------------------------------------------------------------------*/
	// Поверхня вікна.
	/* Поверхня - це ніщо інше, як покажчик на структуру SDL_Surface,
	якась 2d площину на якій відбуватиметься ігровий дію. */
	SDL_Surface *SCREEN;
	SDL_Surface *icon;
	// запускаємо програму у вікні / на повний екран.
	uint Full_screen;
	// Палітра 16 / 32 розрядна
	uint Pal;
	/* --------------------------------------------------------------------------------------*/
public:
	// Конструктор за замовчуванням.
	Heart_of_the_bankomat();
	void run_the_bankomat(void);
	// Завантаження параметрів з файлу, для класу Heart_of_the_game
	bool LoadFromFile(std::string fileName);
	// Ініціалізація всіх потрібних систем SDL / OpenGL.
	bool Init_sdl_opengl(void);
	// Завантаження зображень з файлів на поверхні SDL_Surface.
	void InitImages(void);
	// Ініціалізація всіх потрібних параметрів
	void Init(void);
	// Наступний метод потрібен для ініціалізації 2д в OpenGl .
	void gl2dMode(void);
	// Дестректор.
	~Heart_of_the_bankomat(void);
};


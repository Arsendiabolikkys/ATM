#pragma once
// Підключаємо header файли SDL бібліотек.
#include "Headers_SDL.h"
// Підключаємо header файли OpenGL бібліотеки.
#include "Headers_OpenGL.h"
#include "Defines.h"
// Відображення тексту
#include "Text.h"
class initialization_card
{
public:
	//задній фон логотипа
	SDL_Surface *bc_logo_S;
	// Музика, яку будемо відтворювати
	Mix_Music *music;
private:
	// Флаги для ігрових циклів і оновлення.
	bool logo_exit, logo_run, enter;
	// шрифт
	TTF_Font *font;			
	// Структури координат.
	SDL_Rect dstrect;
	// екземпляр структури, щоб обробляти події.
	SDL_Event Event;
public:
	// конструктор за замовчуванням.
	initialization_card();
	// Завантаження матеріалів з файлів.
	void loading_card(SDL_Surface *SCREEN);
	// Завантаження зображень з файлів на поверхні SDL_Surface.
	void InitImages(void);
	// завантажуємо музику
	void load_audio(void);
	int load_card(SDL_Surface *SCREEN);
	// DrawBG_logo () відображає задній фон на екран.
	void DrawBG_logo(SDL_Surface *SCREEN);
	// DrawIMG () малює зображення на екран в заданій позиції.
	void DrawIMG(SDL_Surface *img, int x, int y, SDL_Surface *SCREEN);
	// Функції DrawScene_logo () малює спрайт. Спочатку затираємо задній фон, потім оновлюємо і малюємо.
	void DrawScene_logo(SDL_Surface *SCREEN);
};


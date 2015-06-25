#pragma once
// ϳ�������� header ����� SDL �������.
#include "Headers_SDL.h"
// ϳ�������� header ����� OpenGL ��������.
#include "Headers_OpenGL.h"
#include "Defines.h"
// ³���������� ������
#include "Text.h"
class initialization_card
{
public:
	//����� ��� ��������
	SDL_Surface *bc_logo_S;
	// ������, ��� ������ �����������
	Mix_Music *music;
private:
	// ����� ��� ������� ����� � ���������.
	bool logo_exit, logo_run, enter;
	// �����
	TTF_Font *font;			
	// ��������� ���������.
	SDL_Rect dstrect;
	// ��������� ���������, ��� ��������� ��䳿.
	SDL_Event Event;
public:
	// ����������� �� �������������.
	initialization_card();
	// ������������ �������� � �����.
	void loading_card(SDL_Surface *SCREEN);
	// ������������ ��������� � ����� �� ������� SDL_Surface.
	void InitImages(void);
	// ����������� ������
	void load_audio(void);
	int load_card(SDL_Surface *SCREEN);
	// DrawBG_logo () �������� ����� ��� �� �����.
	void DrawBG_logo(SDL_Surface *SCREEN);
	// DrawIMG () ����� ���������� �� ����� � ������ �������.
	void DrawIMG(SDL_Surface *img, int x, int y, SDL_Surface *SCREEN);
	// ������� DrawScene_logo () ����� ������. �������� �������� ����� ���, ���� ��������� � �������.
	void DrawScene_logo(SDL_Surface *SCREEN);
};


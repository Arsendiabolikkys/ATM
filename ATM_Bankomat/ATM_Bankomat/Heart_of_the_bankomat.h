#pragma once
/* --------------------------------------------------------------------------------------*/
//									ϳ�������� ��������.								   //
/* --------------------------------------------------------------------------------------*/
#include <string>
#include <fstream>
#include <iostream>
#include <windows.h>
// ϳ�������� header ����� SDL �������.
#include "Headers_SDL.h"
// ϳ�������� header ����� OpenGL ��������.
#include "Headers_OpenGL.h"
#include "Defines.h"
#include "initialization_card.h"
typedef unsigned int uint;
class Heart_of_the_bankomat
{
private:
	/* --------------------------------------------------------------------------------------*/
	// �������� ����.
	/* �������� - �� ��� ����, �� �������� �� ��������� SDL_Surface,
	����� 2d ������� �� ��� �������������� ������� ��. */
	SDL_Surface *SCREEN;
	SDL_Surface *icon;
	// ��������� �������� � ��� / �� ������ �����.
	uint Full_screen;
	// ������ 16 / 32 ��������
	uint Pal;
	/* --------------------------------------------------------------------------------------*/
public:
	// ����������� �� �������������.
	Heart_of_the_bankomat();
	void run_the_bankomat(void);
	// ������������ ��������� � �����, ��� ����� Heart_of_the_game
	bool LoadFromFile(std::string fileName);
	// ����������� ��� �������� ������ SDL / OpenGL.
	bool Init_sdl_opengl(void);
	// ������������ ��������� � ����� �� ������� SDL_Surface.
	void InitImages(void);
	// ����������� ��� �������� ���������
	void Init(void);
	// ��������� ����� ������� ��� ����������� 2� � OpenGl .
	void gl2dMode(void);
	// ����������.
	~Heart_of_the_bankomat(void);
};


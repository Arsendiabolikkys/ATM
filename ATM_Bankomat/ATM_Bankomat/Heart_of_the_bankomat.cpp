#include "Heart_of_the_bankomat.h"


// ����������� �� �������������.
Heart_of_the_bankomat::Heart_of_the_bankomat(void)
{
	// �������� ����.
	SCREEN = NULL;
	icon = NULL;
	// ��������� �������� � ���(0) / �� ������ �����(1).
	Full_screen = 0;
	// ������ 16 / 32 ��������
	Pal = 32;
}
// ����������.
Heart_of_the_bankomat::~Heart_of_the_bankomat(void)
{
	// ������� �������, SDL_Surface.
	SDL_FreeSurface(icon);
	SDL_FreeSurface(SCREEN);
	// ������� ������ Simple Directmedia Library
	SDL_Quit();
}
void Heart_of_the_bankomat::run_the_bankomat(void)
{
	// ������������ ��������� � �����
	if (LoadFromFile("resources/config/settings.txt") == false)
	{
		// �������� ��������� � ���(0) / �� ������ �����(1).
		Full_screen = 0;
	}	
	// ����������� ��� �������� ������ SDL / OpenGL.
	Init_sdl_opengl();
	// ������������ ��������� � ����� �� ������� SDL_Surface.
	InitImages();
	// ����������� ��� �������� ���������.
	Init();
	// ��'��� �� ����. 
	initialization_card Initialization_card;
	Initialization_card.loading_card(SCREEN);
}
// ������������ ��������� � �����
bool Heart_of_the_bankomat::LoadFromFile(std::string fileName)
{
	std::ifstream fin;
	std::string line, set, val;
	fin.open(fileName.data());
	if (fin.is_open())
	{
		while (!fin.eof())
		{
			getline(fin, line);
			// �������������� npos �� ��������� "��� �� ���� �����"
			if (line.find('=') != std::string::npos)
			{
				// ������� �������
				set = line.substr(0, line.find('='));
				val = line.substr(line.find('=') + 1);
				if (set == "Full_screen")
				{
					//	������� atof () - ���������� �����, �� ���������� ���������� val, � �������� ���� double
					Full_screen = (uint)atof(val.data());
					if (!(Full_screen == 0 || Full_screen == 1))
					{
						Full_screen = 0;
					}
				}
			}
		}
		fin.close();
		// ������� ��� �������� �������� (true)
		return true;
	}
	else
		// ������� ��� �������� �� �������� (false)
		return false;
}
// ����������� ��� �������� ������ SDL / OpenGL.
bool Heart_of_the_bankomat::Init_sdl_opengl(void)
{
	/*	SDL_INIT_VIDEO - ������������ ���� ���������.
	SDL_INIT_AUDIO - ������������ ���� ���������.	*/
	if (SDL_Init(SDL_INIT_VIDEO | SDL_INIT_AUDIO) != 0)
	{
		fprintf(stderr, "Unable to initialize SDL: %s\n", SDL_GetError());
		// ������� ��� �������� �� �������� (false)
		return false;
	}
	else
		if (SDL_Init(SDL_INIT_VIDEO | SDL_INIT_AUDIO) == 0)
		{
			// �������� �������� ����� � OpenGL
			SDL_GL_SetAttribute(SDL_GL_DOUBLEBUFFER, 1);

			// ���������� ������ SDL_OPENGL (���� �������� ��� ��, �� SDL ����������� OpenGL).
			/*
			������ ����� ������� ������, ������, ������� ������� � ������� �����������.
			��������������� ����� �������� "|".
			SDL_OPENGL - ������� �������� OpenGL.
			SDL_RESIZABLE - ����� ����� ����� ����, ���������� �� ���� �����
			SDL_FULLSCREEN - SDL ���������� ������������� � ������������� �����.
			���� � ������� ������ ����� ������ �� ���� ������������, �� ���� �������
			��������� ������ �����, � ���������� ���� ������������� �� ������� �����.
			SDL_OPENGLBLIT - ����� ������� �������� OpenGL, ��� �������� blitting-��������.
			������� �������� (2D) ���� ���� �����-����� (���������) � �������
			������������ ����� �� ��������� SDL_UpdateRects.
			SDL_HWSURFACE - ������� ���� �������� � ���'�� ���������
			SDL_DOUBLEBUF - ������ �������� �����. �� ������� ����� ����� � �������� SDL_HWSURFACE
			*/

			// �������� �����
			TTF_Init();

			Mix_OpenAudio(44100, MIX_DEFAULT_FORMAT, 2, 4096);

			//��������� �������� � ���
			if (Full_screen == 0)
			{
				if ((SCREEN = SDL_SetVideoMode(WINDOW_WIDTH, WINDOW_HEIGHT, Pal,
					SDL_HWSURFACE | SDL_DOUBLEBUF /*| SDL_RESIZABLE*/ | SDL_OPENGL | SDL_OPENGLBLIT)) == NULL)
				{
					fprintf(stderr, "Unable to set video mode: %s\n", SDL_GetError());
					SDL_Quit();
					// ������� ��� �������� �� �������� (false)
					return false;
				}
			}
			else
				// ��������� �������� �� ������ �����.
				if (Full_screen == 1)
				{
					if ((SCREEN = SDL_SetVideoMode(WINDOW_WIDTH, WINDOW_HEIGHT, Pal,
						SDL_HWSURFACE | SDL_DOUBLEBUF | SDL_OPENGL | SDL_OPENGLBLIT | SDL_FULLSCREEN)) == NULL)
					{
						fprintf(stderr, "Unable to set video mode: %s\n", SDL_GetError());
						SDL_Quit();
						// ������� ��� �������� �� �������� (false)
						return false;
					}
				}
			// ������� ��� �������� �������� (true)
			return true;
		}
	// ������� ��� �������� �������� (true)
	return true;
}
// ������������ ��������� � ����� �� ������� SDL_Surface.
void Heart_of_the_bankomat::InitImages()
{
	// ������ �������� (� �������� �� ������������)
	icon = SDL_LoadBMP("resources/textures/icon.bmp");
}
// ��������� ����� ������� ��� ���������� 2� � OpenGl .
void Heart_of_the_bankomat::gl2dMode(void)
{
	// glMatrixMode () ���������� ����� ������� �������� ������������.
	glMatrixMode(GL_PROJECTION);
	// glLoadIdentity () ������ ������� ������� �������� ������������ �� ��������.
	glLoadIdentity();
	/* glOrtho ( ) - ���������� ����� �������������� ( ������������ ) ������������.
	�� ������ , �� ���������� ���� ���������� �� � ������� . 6 ���������
	���� GLdouble (��� ������ double ) : left , right , bottom , top , near , far
	���������� ���������� �������� ��� , ����� , ������ , ������� , �������
	� ������� ������ �������� , ����� ��� , �� ��������� �� ���� ������ ,
	���������� �� ����. */
	glOrtho(0, WINDOW_WIDTH, WINDOW_HEIGHT, 0, -1, 1);
	// glMatrixMode () ���������� ����� ������� �������� ������������.
	glMatrixMode(GL_MODELVIEW);
	// glLoadIdentity () ������ ������� ������� �������� ������������ �� ��������.
	glLoadIdentity();
}
// ����������� ��� �������� ���������
void Heart_of_the_bankomat::Init()
{
	// ��������� ����
	// ����� � ��������� ���� � ����� �� ������������ � ������
	SDL_WM_SetCaption(WINDOW_TITLE, WINDOW_TITLE);
	SDL_WM_SetIcon(icon, NULL);
	// ����������� 2�
	gl2dMode();
	// ������ ���� �������� ���� (R, G, B, alpha) - ������
	glClearColor(0, 0, 0, 0);
	// �������� ����� ���������������
	glEnable(GL_TEXTURE_2D);
	// �������� ������������ "�����-������"
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	glEnable(GL_BLEND);
}
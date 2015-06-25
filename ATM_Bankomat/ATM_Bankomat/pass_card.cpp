#include "pass_card.h"

Text text1;
// конструктор за замовчуванням.
pass_card::pass_card()
{
	bc_logo_S = NULL;
	music = NULL;
	logo_exit = false;
	logo_run = true;
	dstrect.x = 0;
	dstrect.y = 0;
}

// Завантаження матеріалів з файлів.
void pass_card::loading_card(SDL_Surface *SCREEN)
{
	// Завантаження зображень з файлів на поверхні SDL_Surface.
	InitImages();
	// завантажуємо музику
	load_audio();

	int key = 1;
	while (key == 1)
	{
		key = load_card(SCREEN);
	}
}
// Завантаження зображень з файлів на поверхні SDL_Surface.
void pass_card::InitImages(void)
{
	//задний фон логотипу
	bc_logo_S = SDL_DisplayFormat(IMG_Load("resources/textures/background/bc_logo_800x600.png"));
	font = TTF_OpenFont("resources/data/font.ttf", WINDOW_WIDTH / 20);	// відкриваємо фонт
}
/* --------------------------------------------------------------------------------------*/
// завантажуємо музику
void pass_card::load_audio(void)
{
	Mix_OpenAudio(44100, MIX_DEFAULT_FORMAT, 2, 4096);
	// завантажуємо музику
	//music = Mix_LoadMUS("resources/audio/logo.wav");
}
int pass_card::load_card(SDL_Surface *SCREEN)
{
	SDL_EnableUNICODE(SDL_ENABLE);
	char number_account[12] = "";
	char number_s_account[12] = "";
	int number_account_key = 0;
	enter = true;
	bool errors = false;
	// Включаємо музику
	Mix_PlayMusic(music, -1);
	// Починається головний цикл програми, за вихід з нього відповідає змінна logo_exit
	while (!logo_exit)
	{
		if (logo_run)
		{
			logo_run = false;
			// DrawBG_logo () відображає задній фон на екран.
			DrawBG_logo(SCREEN);
			while (enter)
			{
				while (SDL_PollEvent(&Event))
				{
					if (Event.type == SDL_KEYDOWN)
					{
						if (strlen(number_account) <12)
						{
							if ((Event.key.keysym.unicode >= (Uint16)'0') && (Event.key.keysym.unicode <= (Uint16)'9'))
							{
								number_account[number_account_key] = (char)Event.key.keysym.unicode;
								number_s_account[number_account_key] = '*';
								number_account_key++;
								if (number_account_key == 12)
								{
									if (strcmp(number_account, "999999999999") == true)
									{
										// Зупинити музику
										Mix_HaltMusic();
										logo_run = false;
										logo_exit = true;
										enter = false;
										//MainMenu.loading_main_menu_(screen, dstrect);
									}
									else
									{
										errors = true;
										for (int i = 0; i < 12; i++)
										{
											--number_account_key;
											number_account[number_account_key] = '\0';
											number_s_account[number_account_key] = '\0';

										}
									}

								}
							}
						}
						if ((Event.key.keysym.sym == SDLK_BACKSPACE) && (strlen(number_account) != 0))
						{
							--number_account_key;
							number_account[number_account_key] = '\0';
							number_s_account[number_account_key] = '\0';
						}
					}
				}
				glColor4f(0.0, 0.0, 0.0, 1);
				if (strlen(number_account) != 0)
				{
					text1.Draw_Text(250, 200, 250, 38, font, number_s_account, 0, 0);
				}
				text1.Draw_Text(150, 140, 800, 38, font, "(введить пароль до карты)", 0, 0);
				if (errors == true)
					text1.Draw_Text(100, 50, 800, 38, font, "(Такой пароль не активный)", 0, 0);
				glColor4f(1, 1, 1, 1);
				glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
				glEnable(GL_TEXTURE_2D);
				// Функції DrawScene () малює спрайт. Спочатку затираємо задній фон, потім оновлюємо і малюємо.
				DrawScene_logo(SCREEN);
				SDL_Delay(100);
			}
		}
	}
	// функція своє завдання виконала ( true )
	return 0;
}
// DrawBG_logo () відображає задній фон на екран.
void pass_card::DrawBG_logo(SDL_Surface *SCREEN)
{
	DrawIMG(bc_logo_S, 0, 0, SCREEN);
}
// DrawIMG () малює зображення на екран в заданій позиції.
void pass_card::DrawIMG(SDL_Surface *img, int x, int y, SDL_Surface *SCREEN)
{
	dstrect.x = x;
	dstrect.y = y;
	SDL_BlitSurface(img, NULL, SCREEN, &dstrect);
}
// Функції DrawScene_logo () малює спрайт. Спочатку затираємо задній фон, потім оновлюємо і малюємо.
void pass_card::DrawScene_logo(SDL_Surface *screen)
{
	SDL_GL_SwapBuffers();
	// SDL_Flip - перемикає буфери, оновлюючи екран.
	SDL_Flip(screen);
}
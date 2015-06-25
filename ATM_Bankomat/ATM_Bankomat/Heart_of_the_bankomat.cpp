#include "Heart_of_the_bankomat.h"


// Конструктор за замовчуванням.
Heart_of_the_bankomat::Heart_of_the_bankomat(void)
{
	// Поверхня вікна.
	SCREEN = NULL;
	icon = NULL;
	// запускаємо програму у вікні(0) / на повний екран(1).
	Full_screen = 0;
	// Палітра 16 / 32 розрядна
	Pal = 32;
}
// Дестректор.
Heart_of_the_bankomat::~Heart_of_the_bankomat(void)
{
	// Звільняє ресурси, SDL_Surface.
	SDL_FreeSurface(icon);
	SDL_FreeSurface(SCREEN);
	// Завершує роботу Simple Directmedia Library
	SDL_Quit();
}
void Heart_of_the_bankomat::run_the_bankomat(void)
{
	// Завантаження параметрів з файлу
	if (LoadFromFile("resources/config/settings.txt") == false)
	{
		// програму запускаємо у вікні(0) / на повний екран(1).
		Full_screen = 0;
	}	
	// Ініціалізація всіх потрібних систем SDL / OpenGL.
	Init_sdl_opengl();
	// Завантаження зображень з файлів на поверхні SDL_Surface.
	InitImages();
	// Ініціалізація всіх потрібних параметрів.
	Init();
	// Об'єкт на клас. 
	initialization_card Initialization_card;
	Initialization_card.loading_card(SCREEN);
}
// Завантаження параметрів з файлу
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
			// Використовують npos як індикатор "все до кінця рядка"
			if (line.find('=') != std::string::npos)
			{
				// повертає підрядок
				set = line.substr(0, line.find('='));
				val = line.substr(line.find('=') + 1);
				if (set == "Full_screen")
				{
					//	Функція atof () - Перетворює рядок, що адресується параметром val, в значення типу double
					Full_screen = (uint)atof(val.data());
					if (!(Full_screen == 0 || Full_screen == 1))
					{
						Full_screen = 0;
					}
				}
			}
		}
		fin.close();
		// функція своє завдання виконала (true)
		return true;
	}
	else
		// Функція своє завдання не виконала (false)
		return false;
}
// Ініціалізація всіх потрібних систем SDL / OpenGL.
bool Heart_of_the_bankomat::Init_sdl_opengl(void)
{
	/*	SDL_INIT_VIDEO - ініціалізувати відео підсистему.
	SDL_INIT_AUDIO - ініціалізувати аудіо підсистему.	*/
	if (SDL_Init(SDL_INIT_VIDEO | SDL_INIT_AUDIO) != 0)
	{
		fprintf(stderr, "Unable to initialize SDL: %s\n", SDL_GetError());
		// Функція своє завдання не виконала (false)
		return false;
	}
	else
		if (SDL_Init(SDL_INIT_VIDEO | SDL_INIT_AUDIO) == 0)
		{
			// Включаємо подвійний буфер з OpenGL
			SDL_GL_SetAttribute(SDL_GL_DOUBLEBUFFER, 1);

			// ініціалізуємо прапор SDL_OPENGL (який говорить про те, що SDL використовує OpenGL).
			/*
			Задаємо нашій поверхні ширину, висоту, глибину кольору і прапори відображення.
			Перераховуються через оператор "|".
			SDL_OPENGL - створює контекст OpenGL.
			SDL_RESIZABLE - можна міняти розмір вікна, потягнувши за його рамку
			SDL_FULLSCREEN - SDL намагається переключитися в повноекранний режим.
			Якщо з якихось причин розмір екрану не може встановитися, то буде обраний
			наступний більший режим, а зображення буде відцентрований на чорному екрані.
			SDL_OPENGLBLIT - також створює контекст OpenGL, але дозволяє blitting-операції.
			Екранна поверхню (2D) може мати альфа-канал (прозорість) і повинна
			оновлюватися тільки за допомогою SDL_UpdateRects.
			SDL_HWSURFACE - створює відео поверхню в пам'яті відеокарти
			SDL_DOUBLEBUF - включає подвійний буфер. Це можливо тільки разом з прапором SDL_HWSURFACE
			*/

			// Ініціюємо шрифт
			TTF_Init();

			Mix_OpenAudio(44100, MIX_DEFAULT_FORMAT, 2, 4096);

			//запускаємо програму у вікні
			if (Full_screen == 0)
			{
				if ((SCREEN = SDL_SetVideoMode(WINDOW_WIDTH, WINDOW_HEIGHT, Pal,
					SDL_HWSURFACE | SDL_DOUBLEBUF /*| SDL_RESIZABLE*/ | SDL_OPENGL | SDL_OPENGLBLIT)) == NULL)
				{
					fprintf(stderr, "Unable to set video mode: %s\n", SDL_GetError());
					SDL_Quit();
					// Функція своє завдання не виконала (false)
					return false;
				}
			}
			else
				// запускаємо програму на повний екран.
				if (Full_screen == 1)
				{
					if ((SCREEN = SDL_SetVideoMode(WINDOW_WIDTH, WINDOW_HEIGHT, Pal,
						SDL_HWSURFACE | SDL_DOUBLEBUF | SDL_OPENGL | SDL_OPENGLBLIT | SDL_FULLSCREEN)) == NULL)
					{
						fprintf(stderr, "Unable to set video mode: %s\n", SDL_GetError());
						SDL_Quit();
						// Функція своє завдання не виконала (false)
						return false;
					}
				}
			// функція своє завдання виконала (true)
			return true;
		}
	// функція своє завдання виконала (true)
	return true;
}
// Завантаження зображень з файлів на поверхні SDL_Surface.
void Heart_of_the_bankomat::InitImages()
{
	// Іконка програми (в Опенбокс не відображається)
	icon = SDL_LoadBMP("resources/textures/icon.bmp");
}
// Наступний метод потрібен для іціалізаціі 2д в OpenGl .
void Heart_of_the_bankomat::gl2dMode(void)
{
	// glMatrixMode () встановлює режим матриці видового перетворення.
	glMatrixMode(GL_PROJECTION);
	// glLoadIdentity () замінює поточну матрицю видового перетворення на одиничну.
	glLoadIdentity();
	/* glOrtho ( ) - встановлює режим ортогонального ( прямокутного ) проектування.
	Це означає , що зображення буде малюватися як в ізометрії . 6 параметрів
	типу GLdouble (або просто double ) : left , right , bottom , top , near , far
	визначають координати відповідно лівої , правої , нижньої , верхньої , ближньої
	і дальньої площин відсікання , тобто все , що опиниться за цими межами ,
	малюватися не буде. */
	glOrtho(0, WINDOW_WIDTH, WINDOW_HEIGHT, 0, -1, 1);
	// glMatrixMode () встановлює режим матриці видового перетворення.
	glMatrixMode(GL_MODELVIEW);
	// glLoadIdentity () замінює поточну матрицю видового перетворення на одиничну.
	glLoadIdentity();
}
// Ініціалізація всіх потрібних параметрів
void Heart_of_the_bankomat::Init()
{
	// Створюємо вікно
	// Текст в заголовку вікна і текст що відображається в панеле
	SDL_WM_SetCaption(WINDOW_TITLE, WINDOW_TITLE);
	SDL_WM_SetIcon(icon, NULL);
	// Ініціалізіруем 2д
	gl2dMode();
	// Задаємо колір заднього фону (R, G, B, alpha) - чорний
	glClearColor(0, 0, 0, 0);
	// Включаємо режим текстурирования
	glEnable(GL_TEXTURE_2D);
	// Включаємо використання "альфа-каналу"
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);
	glEnable(GL_BLEND);
}
/* --------------------------------------------------------------------------------------*/
//								ϳ�������� ����� ��������.							   //
/* --------------------------------------------------------------------------------------*/
#include "Text.h"
/* --------------------------------------------------------------------------------------*/
// �������� ����� � �����
std::string Text::IntToStr(int i)
{
	char buf[16];
	_snprintf(buf, sizeof(buf), "%i", i);
	return std::string(buf);
}
/* --------------------------------------------------------------------------------------*/
// �������� ������� ��� ����������� ������
void Text::GlTxt(int sdl_dx, int sdl_dy, TTF_Font *font,
	SDL_Color textColor, const char text[], GLuint texture)
{
	SDL_Surface *temp = NULL, *tempb = NULL;
	int w, h;
	Uint32 rmask, gmask, bmask, amask;
#if SDL_BYTEORDER == SDL_BIG_ENDIAN
	rmask = 0xff000000;
	gmask = 0x00ff0000;
	bmask = 0x0000ff00;
	amask = 0x000000ff;
#else
	rmask = 0x000000ff;
	gmask = 0x0000ff00;
	bmask = 0x00ff0000;
	amask = 0xff000000;
#endif
	if (font == NULL)
	{
		std::cout << SDL_GetError() << std::endl;;
		SDL_FreeSurface(temp);
		SDL_FreeSurface(tempb);
	}
	temp = TTF_RenderText_Blended(font, text, textColor);
	SDL_SetAlpha(temp, 0, 0);
	tempb = SDL_CreateRGBSurface(0, sdl_dx, sdl_dy, 32, rmask, gmask, bmask, amask);
	TTF_SizeUTF8(font, text, &w, &h);
	SDL_Rect src, dst;
	src.x = 0;
	src.y = 0;
	src.w = sdl_dx;	//w;
	src.h = sdl_dy;	//h;
	dst.x = 0;
	dst.y = 0;
	dst.w = sdl_dx;	//w;
	dst.h = sdl_dy;	//h;
	SDL_BlitSurface(temp, &src, tempb, &dst);
	glBindTexture(GL_TEXTURE_2D, texture);
	gluBuild2DMipmaps(GL_TEXTURE_2D, GL_RGBA,
		tempb->w, tempb->h,
		GL_RGBA, GL_UNSIGNED_BYTE,
		tempb->pixels);
	SDL_FreeSurface(temp);
	SDL_FreeSurface(tempb);
}
/* --------------------------------------------------------------------------------------*/
// ������� ��� ����������� ������
void Text::Draw_Text(float x, float y, float dX, float dY,
	TTF_Font *fonts,//������������ *.ttf ����
	std::string text,//��� ��������� �����
	float delta, int center)
{
	SDL_Color txtColorWhite = { 255, 255, 255 };	// ���� ������, �� ������������� - ����
	// ���� ������� ��������������� ����� ���� ��� ������, �� ����� ��������� "��������� ������"
	GLuint t_txt;	// ��������� �������� ��������	 
	glGenTextures(1, &t_txt);	// ���������� �������� ��������
	GlTxt(dX, dY, fonts, txtColorWhite, text.c_str(), t_txt);
	// ������� ������ � ��������� ���������
	glBindTexture(GL_TEXTURE_2D, t_txt);
	DrawTXT(x, y, dX, dY, delta, center);
	glDeleteTextures(1, &t_txt);	// ��������� ����������� �������� 
}
/* --------------------------------------------------------------------------------------*/
/* ��������� ��� ��������� ������������������ ������� � ����������� x, y;
������� dX, dY; ����������� �� delta �������. ���� ������� ��� �����,
��� - ���� ������. �� ��������� ������ ��������������� ��� ��������� ����
��� ������� / �����, �������� ���� ����������� ��� ������� ���������� ������. */
void Text::DrawTXT(float x, float y, float dX, float dY, float delta, int center)
{
	glEnable(GL_TEXTURE_2D);
	glLoadIdentity();
	glTranslatef(x, y, 0);		// ������ ���� ������������
	glRotatef(delta, 0, 0, -1); // ����������� �� delta �������
	// ���� ������� ��'���� ���������� ������� ���� ������, �� ������ ��� ��������� �� 0.5dX � 0.5dY
	if (center == 1)
	{
		glTranslatef(-dX / 2, -dY / 2, 0);
	}
	// ������� ����������������� ������, �������� ���������� glTexCoord2i ��������� �% / 100
	glBegin(GL_QUADS);
	// ������ ���� ���
	glTexCoord2i(0, 0);
	glVertex2f(0, 0);
	// ����� ���� ���
	glTexCoord2i(0, 1);
	glVertex2f(0, dY);
	// ����� ������ ���
	glTexCoord2i(1, 1);
	glVertex2f(dX, dY);
	// ������ ������ ���
	glTexCoord2i(1, 0);
	glVertex2f(dX, 0);
	glEnd();
	glLoadIdentity();
}
/* --------------------------------------------------------------------------------------*/
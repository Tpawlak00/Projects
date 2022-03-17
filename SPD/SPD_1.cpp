#include <iostream>
#include <fstream>
using namespace std;
struct task
{
	int id;
	int r;
	int p;
	int q;
};
void wyswietl_kol(task tablica[], int size)
{
	cout << "Kolejnosc wykonywania zadan:" << endl;
	for (int i = 0; i < size; i++)
	{
		cout << tablica[i].id << " ";
	}
	cout << endl;
}
int dlugosc(task tablica[], int size)
{
	int m = 0,c=0;
	for (int i = 0; i < size; i++)
	{
		m = max(m, tablica[i].r) + tablica[i].p;
		c = max(c,m + tablica[i].q);
	}
	return c;
}
void zamien(task tab[], int a, int b)
{
	task pom;
	pom.id = tab[a].id;
	pom.r = tab[a].r;
	pom.q = tab[a].q;
	pom.p = tab[a].p;
	tab[a].id = tab[b].id;
	tab[a].r = tab[b].r;
	tab[a].p = tab[b].p;
	tab[a].q = tab[b].q;
	tab[b].id = pom.id;
	tab[b].r = pom.r;
	tab[b].p = pom.p;
	tab[b].q = pom.q;
}
void ustawianie(task tablica[], int size)
{
	int length = dlugosc(tablica, size);
	int length2;
	for (int k = 0; k < 10; k++)
	{
		for (int i = 0; i < size; i++)
		{
			for (int j = 0; j < size; j++)
			{
				zamien(tablica, i, j);
				length2 = dlugosc(tablica, size);
				if (length2 > length)
				{
					zamien(tablica, i, j);
				}
				else
				{
					length = length2;
				}
			}
		}
	}
}
int main()
{
	char znak;
	string s;
	string s1[4] = { "data.1","data.2","data.3","data.4" };
	int n, suma = 0;
	int tab[4];
	task tablica[100];
	ifstream wczytaj("data.txt");
	for (int i = 0; i < 4; i++)
	{
		cout << "Zbior danych: " << i + 1 << endl;
		while (s != s1[i])
		{
			wczytaj >> s;
		}
		wczytaj >> n;
		for (int i = 0; i < n; i++)
		{
			tablica[i].id = i + 1;
			wczytaj >> tablica[i].r;
			wczytaj >> tablica[i].p;
			wczytaj >> tablica[i].q;
		}
		ustawianie(tablica, n);
		tab[i] = dlugosc(tablica, n);
		suma += dlugosc(tablica, n);
		wyswietl_kol(tablica, n);
	}
	for (int i = 0; i < 4; i++)
	{
		cout << "Dlugosc " << i+1 << " zbioru danych: " << tab[i] << endl;
	}
	cout << "Laczna dlugosc czasu trwania: " << suma << endl;
	wczytaj.close();
	cin >> znak;
}
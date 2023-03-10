# Wyszukiwanie książek przy pomocy tagów

## Cel projektu
Celem projektu było stworzenie aplikacji internetowej umożliwiającej wyszukiwanie stron internetowych na bazie wprowadzonej frazy. Wymaganym było, aby oferowała ona funkcję proponowania słów bazując na wprowadzonym przez użytkownika ciągu znaków. Dane spośród, których mają być wyszukiwane strony powinny zostać uzyskane poprzez scrapowanie stron za pomocą napisanego bota. Dodatkową funkcją programu powinno być wyszukiwanie stron za pomocą predefiniowanych kategorii, które pozwolą ograniczyć zbiór zwracanych wyników.

## Opis projektu 
Projekt składa się z kilku głównych komponentów: frontend, backend, usługi Azure Functions przeznaczonej do scrapowania stron internetowych, bazy danych oraz kilku pomniejszych elementów. Warstwa front-endowa została napisana przy pomocy biblioteki react 17.0.1 w języku javascript. Do pobierania danych zostało użyte narzędzie React Query, które pomaga obsługiwać żądania wysyłane do serwera. Warstwa back-endowa została natomiast napisana w języku C# w wersji 10.0. Wystawia ona REST API z dwoma endpointami. Pierwszy z nich służy do wyszukiwania rezultatów za pomocą przekazanej przez użytkownika frazy. Drugi natomiast jest wykorzystywany przez Azure Functions i pozwala dodać zbiór nowych stron. Dodatkowo serwer łączy się z usługą Language Service, która dostarcza możliwość analizowania tekstu i wydobywania z niego słów kluczowych. Do przechowywania danych używana jest usługa SQL Azure. Warto wspomnieć, że funkcjonalności udostępniane przez Azure Functions zostały napisane przy pomocy języka Python. Spełniają one dwie istotne role. Pierwsza z nich polega na wykorzystywaniu sztucznej inteligencji do proponowania słów w polu wyszukiwania. Druga natomiast odpowiada za pobranie nowych stron i przesłaniu ich do back-endu.

## Opis działania 
Użytkownik po wejściu na stronę może rozpocząć wyszukiwanie odpowiedzi na interesujące go pytanie. W tym celu musi wprowadzić w pole wyszukiwania poszukiwaną frazę. W czasie jej wpisywania wysyłane są żądania do Azure Functions i zwracane jest sugerowane słowo. Użytkownik może je wybrać lub dalej wpisywać szukaną frazę. Po wybraniu kategorii, za pomocą której ma być przeprowadzone szukanie rozwiązania, użytkownik klika przycisk szukaj. W tym momencie wysyłane jest żądanie do API. Serwer po otrzymaniu szukanej frazy wykorzystuje Language Service do wydobycia słów kluczowych. Po ich otrzymaniu filtruje w bazie danych strony, które zawierają poszukiwaną frazę. Pobiera przy tym jedynie tą która posiada najlepsze dopasowanie, czyli posiada najwięcej identycznych słów kluczowych z poszukiwanym tekstem. Następnie zwraca wyszukaną stronę, a front-end wyświetla ją za pomocą elementu iframe.

Warto także wspomnieć o mechanizmie dodawania nowych stron internetowych. Są one pobierane poprzez scrapowanie wstępnie wybranych stron. Następnie wysyłane są do back-endu. Serwer najpierw weryfikuje, czy przesłana strona już istnieje. Jeśli nie to z jej tytułu wyciągane są słowa kluczowe. Jeżeli któreś z nich nie istnieją to są dodawane do bazy danych. Potem dana strona jest zapisywana i przyporządkowywane są do niej powiązane słowa.  

## Opis serwisów

__App Service__ - Wykorzystany do zhostowania aplikacji front-endowej oraz back-endowej.

__SQL Azure__ – Wykorzystany do przechowywania szczegółów stron internetowych (url, tytuł strony oraz obraz) oraz słów kluczowych (nazwa słowa kluczowego). 

__Language Service__ - Służy do wydobywania istotnych słów z przekazanego tekstu. 

__Azure Functions__ – Wykorzystywany do zwracania sugerowanych słów oraz do scrapowania stron i wysyłania ich do endpointu API. 

__Azure Key Vault__ - Przechowuje sekrety takie jak np. adres url Language Service oraz jego klucz dostępowy.

__Azure Blob Storage__ - Służy do przechowywania zdjęć.

__Azure DevOps__ - Wykorzystywany do zarządzania projektem poprzez planowanie sprintów oraz tworzenie i przydzielanie zadań.

## Authors

W skład zespołu projektowego wchodzą:

- [Fereniec Michał](https://github.com/Michal2390)
- [Kasprzak Arkadiusz](https://github.com/Kasprzak-Arkadiusz)
- [Kowalczyk Marcin](https://github.com/kowalczy)
- [Milewski Adrian](https://github.com/milewsa3)
- [Komorowski Jakub](https://github.com/KomorowskiKuba)
- [Modzelewska Patrycja](https://github.com/modzelpatrycja)

## Architektura
![architecture](/Readme/Architecture.png)

## Demo

https://www.youtube.com/watch?v=Wqzi3TPMhUU

## Podział obowiązków:

- Arkadiusz Kasprzak - backend, baza danych
- Jakub Komorowski - AI (bot scrapujący tytuły i linki, ai podpowiadające następne słowo)
- Adrian Milewski - frontend
- Patrycja Modzelewska - frontend
- Marcin Kowalczyk - zarządzanie projektem, planowanie, rozdzielenie zadań
- Michał Fereniec - nagrywanie filmu, zarządzanie zasobami na platformie Azure

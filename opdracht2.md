### Opdracht week 4 : taalmodellen

# Opdracht 2

Je gaat zelf een originele toepassing bedenken en bouwen waarin een taalmodel is geïntegreerd. Lees de exacte opdracht en inlevervoorwaarden in de cursushandleiding.

<br><br><br>

### Live zetten

Voor de lessen en inleveropdrachten kan je jouw frontend en backend lokaal draaien op je eigen computer. Voor het expert level kan je je project live online zetten. Zie [Les 9](/les9/).

<br><br><br>

## Voorbeeld 1

![poemclock](./images/poemclock.png) <br>
De Poem Clock schrijft elke 15 minuten een gedicht over de huidige tijd

<Br>

## Voorbeeld 2

De game quiz stelt 10 vragen over recente games. Elk antwoord staat in een button waar je op kan klikken. De AI weet of het antwoord goed is en houdt een score bij. 

#### System prompt
```
kan je een quizvraag met 4 antwoorden bedenken over recente videogames? 1 antwoord is goed
```
#### Resultaat
```
Welke van de volgende games is ontwikkeld door de studio FromSoftware?

A) The Legend of Zelda: Breath of the Wild
B) Dark Souls III
C) Super Mario Odyssey
D) Fortnite
```
<br>

## Inspiratie

- Vraag je LLM om een goed idee!
- Je chatbot hoeft niet altijd op een vraag te wachten. Je kan ook automatisch (bv. via `setInterval`) prompts uitvoeren, om een vraag aan je gebruiker te stellen.
- [Use cases for LLM models](https://www.projectpro.io/article/large-language-model-use-cases-and-applications/887)
- Maak een online chatbot die vragen kan beantwoorden over een bepaald onderwerp. Voed de chatbot met veel data over dat onderwerp. 
- Geef je chatbot een persoonlijkheid die bij het onderwerp past, bijvoorbeeld door alle gesproken lines van een filmkarakter uit een filmscript te halen. [Harry Potter dataset](https://www.kaggle.com/datasets/gulsahdemiryurek/harry-potter-dataset).
- Plaats je chatbot op discord, [tutorial hier](https://dev.to/rtagliavia/how-to-create-a-discord-bot-with-discordjs-and-nodejs-plb) en [hier](https://www.freecodecamp.org/news/discord-ai-chatbot/)
- Haal informatie op van een nieuws-API, weer-API of [een van de vele online api's](https://apilist.fun) en gebruik ChatGPT om dit uit te leggen, kledingtips te geven of om te klagen over het weer!
- Maak een quiz die creative vragen stelt over je favoriete onderwerp en hou een score bij.
- Gebruik een taalmodel om een afspeellijst te maken voor een bepaalde situatie, en gebruik de Spotify API om die lijst af te spelen.
- Maak een taal-game zoals [Codenames](https://www.whitegoblingames.com/game/codenames/) na met ChatGPT.
- Maak een virtuele social media persoonlijkheid die elke dag een post publiceert.
- Link een taalmodel aan een smart home, zodat je je huis kan besturen met taal. Hier kan je [IFTTT](https://ifttt.com) voor gebruiken.
- Maak een tamagotchi game waarbij de tamagotchi echt een persoonlijkheid heeft en opmerkingen kan maken over hoe goed je voor de tamagotchi zorgt.
- Maak een kook-app die recepten kan bedenken of recepten kan omzetten voor vegetariërs / veganisten.
- Maak een dagboek-app waar je tegen kan praten, de app noteert per dag een samenvatting van wat je verteld hebt. Er is een maandoverzicht waarin je nog korter snel kan zien wat er allemaal die maand gebeurd is.
- Maak een muziek oefen-app met [tone.js](https://tonejs.github.io) en gebruik ChatGPT om de oefeningen te maken (bijvoorbeeld een akkoordprogressie).
- Of laat het taalmodel automatisch beats en melodiën bedenken die je afspeelt met [tone.js](https://tonejs.github.io)
- Maak een assistent voor het leren van een taal. Geef het taalmodel instructies om een score bij te houden of te onthouden waar je wel en niet goed in bent.
- Maak een standaard-app zoals een `to-do-lijst` slim door vrije tekstinvoer toe te staan. ChatGPT kan de invoer interpreteren *(als je zegt `Ik heb vandaag de kat gevoerd!` wordt `kat voeren` van de lijst verwijderd).*
- Combineer ChatGPT met een fotoherkennings-API, bijvoorbeeld om recepten te geven voor voedsel in je koelkast, of door iets te zeggen als een kat voor de webcam loopt.
- Gebruik ChatGPT in een spel om een verhaal te creëren of hints te geven. Je kunt vooruitgang boeken in een quest door tekst te gebruiken! *(de speler heeft het zwaard aan de koning geleverd, wat is het volgende?)*
- Gebruik een hardwareboard zoals Adafruit Feather of een Raspberry Pi om een fysiek apparaat aan ChatGPT te koppelen.
- Doe onderzoek naar image en speech generation voor je applicatie. Je kunt de [browser speech API](./snippets/speech.md) gebruiken om de resultaten terug te spreken naar de gebruiker.
- Experimenteer met de persoonlijkheid van de AI, het hoeft niet altijd een vriendelijke behulpzame assistent te zijn!
- Bekijk [OpenGPT](https://www.opengpt.com) om inspiratie op te doen voor AI assistants.
- Kan je een taalmodel een bordpsel mee laten spelen? Begrijpt het model de staat van het bord van een eenvoudig bordspel? Kan een taalmodel een dobbelsteen rollen? Dit is interessant om te onderzoeken!
- Expert level: gebruik [tools, functions of agents](https://platform.openai.com/docs/guides/function-calling) en [LangChain Function calling](https://js.langchain.com/docs/how_to/tool_calling/) om het taalmodel *code in jouw eigen programma* te laten uitvoeren, zoals het besturen van een game karakter 🤯.
- Maak dit [Raspberry Pi Fotolijstje](https://github.com/fatihak/InkyPi) aanstuurbaar met een taalmodel. Vraag om een foto van een kat met een piratenhoedje!
- Als dat werkt zou je taalmodel ook een robot (circuit playground express, raspberry pi, arduino of esp32) moeten kunnen besturen!

<br><br><br>

## Troubleshooting

- Gebruik de praktijklessen om met problemen bij je docent langs te lopen
- Als de Azure OpenAI API key niet werkt, geef dit dan zo snel mogelijk aan bij je docenten.
- Je kan dit tijdelijk oplossen door even de [Fake LLM](https://js.langchain.com/docs/integrations/chat/fake) te gebruiken, zodat je in elk geval door kan werken aan je app.
- Je kan ook besluiten om een eigen `OpenAI API Key` aan te vragen.

<br><br><br>

## Links

- [LangChain](https://js.langchain.com/docs/get_started/quickstart)
- [Prompt Engineering](https://platform.openai.com/docs/guides/prompt-engineering)
- [Browser speech](./snippets/speech.md)
- [OpenAI API](https://platform.openai.com/docs/introduction)


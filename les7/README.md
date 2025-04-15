# Les 7

- Prompt engineering
- Temperature
- Context window en chat history
- Streaming
- Toepassing bedenken
- Documentatie

<br><br><br>

## Prompt engineering

Dit houdt in dat je de vraag van de eindgebruiker uitbreidt met extra taalinstructies, om zo een beter resultaat terug te krijgen van het model. Bijvoorbeeld:

```js
let prompt = `Tell me how to take care of my banana plant`
let engineeredPrompt = `You are a plant expert. Answer the following question: ${prompt} as short as you can.`
let result = await model.invoke(engineeredPrompt)
```
Je kan de lengte van het antwoord bepalen met `maxTokens`. 

```js
const model = new AzureChatOpenAI({
    temperature: 0.5,
    verbose: false,
    maxTokens: 30 // Adjust as needed
});
```

- [Prompt Engineering](https://platform.openai.com/docs/guides/prompt-engineering)
- [Langchain Functies](https://js.langchain.com/docs/modules/model_io/prompts/quick_start) om prompt templates te schrijven.

#### Itereren

Werkt een prompt niet zoals je gehoopt had, lees hem dan nog eens goed en bedenk waarom de prompt verkeerd geïnterpreteerd zou kunnen worden. Je kan aan ChatGPT vragen om de prompt te verbeteren *(promptception🤯)*.

#### Technieken

* Geef de AI een rol die past bij wat je aan hem gaat vragen.
  *  ‘Je bent een zeer creatieve kinderboekenschrijver’
  *  ‘Je bent een extreem nauwkeurige wetenschapper gespecialiseerd in…’

* Wees duidelijk. Dat is bij taalmodellen niet hetzelfde als kort. Meer context is beter.

* Geef een voorbeeld van wat je verwacht.
  * ‘Een goede vraag voor deze dierenquiz zou zijn: hoeveel bulten heeft een kameel?’
  * ‘Als ik zeg dat de kat geen honger meer heeft, dan kan ‘kat eten geven’ uit de todo-lijst.

* Vraag om een output formaat zoals JSON, en geef daar ook een voorbeeld van.
  * Dit is zowel handig om het uit te kunnen lezen met een script, maar zorgt ook dat je kunt bepalen wat je allemaal in je antwoord wilt hebben.

* Formatteer zelf je antwoord om het model te helpen gegevens te vinden
  * ‘Geef een samenvatting van de tekst tussen <context></context>’.
  * ‘Het onderwerp voor het verhaal dat je moet verzinnen staat tussen [onderwerp][/onderwerp]’.

* Begeleid het ‘denkproces’ van het model
  * ‘De gebruiker wil graag een recept. Bedenk welke gerechten populair zijn in de Indische keuken die snel klaar zijn. De gebruiker heeft de volgende ingrediënten …. Kies een Indisch recept dat je daarmee zou kunnen maken in 30 minuten.’


<Br><br><br>

## Temperature

Als je aan het debuggen bent is het handig om voorspelbare resultaten te krijgen zodat je kunt zien of je prompt beter wordt. De onvoorspelbaarheid van een model regelen we met de `temperature`. Zet deze op 0.0 voor erg voorspelbare resultaten. 

Je kan dit juist een hoge waarde geven als je creatieve en onvoorspelbare resultaten wil krijgen. Een waarde van 2 is heel creatief. 

```javascript
const model = new AzureChatOpenAI({ temperature: 0.3 })
```
Je kan ook per prompt de temperature meegeven:

```js
const joke = await model.invoke({
      prompt: "Tell me a Javascript joke!",
      temperature: 2,
});
console.log(joke.content)
```


<br><br><br>

## Context window en chat history

Een chat model onthoudt niet automatisch de hele discussie die je met het model voert. Een vraag zoals "wat bedoel je daarmee" zal dus niet automatisch goed beantwoord worden. Om een dialoog te voeren moet je telkens de hele chat history meesturen met elke nieuwe prompt. Het antwoord van de vorige prompt moet je ook toevoegen aan de history. Een chat model maakt onderscheid in de rol van degene die het bericht stuurt:

- `system` : Dit is een systeem instructie waarmee je kan aangeven hoe het chat model zich moet gedragen.
- `human` : Dit is het bericht van de eindgebruiker.
- `ai` (of `assistant`) : Hiermee kan je aangeven hoe het chat model zelf heeft gereageerd (of zou moeten reageren) op een vraag.

In dit code voorbeeld maken we een array met de history. Vragen van de gebruiker, en antwoorden van de chatbot worden met de juiste `role` toegevoegd.

```js
let messages = [
    ["system", "You are a neanderthal assistant. End every sentence with a different random grunt"],
    ["human", "How is the solar system composed?"],
]
const chat1 = await model.invoke(messages)
console.log(chat1.content)
```
Het antwoord van de ai voeg je toe aan de `messages` array met de juiste rol, gevolgd door een nieuwe vraag. Je geeft de hele array door aan het taalmodel.
```js
messages.push(
    ["assistant", chat1.content],
    ["human", "Can you explain that a bit more?"]
)
const chat2 = await model.invoke(messages)
console.log(chat2.content)
```

<br><br><br>

## Chat history per client

In bovenstaand voorbeeld is de chat history een variabele binnen de node applicatie. Dit klopt eigenlijk niet, want als er meerdere clients tegelijk verbonden zijn, gaan hun chat histories door elkaar lopen.

- Hou de chat history bij *in de browser* in plaats van op de server, en geef die telkens mee, met elke prompt.
- Je hebt de optie om de chat history in `localStorage` op te slaan. Dan blijft de history ook bewaard nadat je de browser afsluit. 

```js
messages.push(["human", "what do you mean by that?"])
localStorage.setItem("myChatHistory", JSON.stringify(messages))
```

<br><Br><br>

### Classes voor roles

Langchain biedt classes aan om de roles wat duidelijker te maken in je code:

```js
import { HumanMessage, SystemMessage, AIMessage } from "langchain/chat_models/messages"

const messages = [
  new SystemMessage("You're a helpful assistant"),
  new HumanMessage("What is the purpose of studying AI?"),
  new AIMessage("It will help you create smarter apps"),
  new HumanMessage("Does that mean I can let AI do all the work?"),
]
```



<br><br><br>

## Streaming

Het kan tijd kosten voordat je een volledig antwoord van een LLM terug krijgt. Vooral bij hele lange antwoorden *(zoals de samenvatting van een boek)* kan het lijken of je user interface niet meer reageert. Om een betere user experience te creeëren kan je streaming gebruiken. Je krijgt dan woord-voor-woord een antwoord terug.

```js
const stream = await model.stream("Write an introduction for a book about a colony of tiny hamsters.")
for await (const chunk of stream) {
    console.log(chunk.content)
}
```

### Streaming in Express

In Express gebruiken we normaal `res.send` (of `res.json`) om een response te sturen. Dit stuurt dan de headers, content en sluit de verbinding met de client. Je kunt dit zelf ook volledig uitschrijven.

```js
// headers, content, en end in één
res.send("Hello World"); 

// hetzelfde maar dan uitgeschreven
res.setHeader("Content-Type", "text/plain");
res.write("Hello World");
res.end();
```
Omdat je na `write` niet automatisch de verbinding sluit, kan je dit gebruiken om je eigen webservice ook streaming te maken.

```js
app.post('/hamstercolony', async (req, res) => {
    const stream = await model.stream("Write an introduction for a book about a colony of tiny hamsters.");
    res.setHeader("Content-Type", "text/plain");
    for await (const chunk of stream) {
        console.log(chunk.content);
        res.write(chunk.content);
    }
    res.end();
}
```

### Streaming in frontend
Met fetch kan je een *readable stream* gebruiken om de response in chuncks af te handelen, in plaats van wachten tot de response helemaal ontvangen is.
* [readablestream en fetch](https://www.loginradius.com/blog/engineering/guest-post/http-streaming-with-nodejs-and-fetch-api/).
* [Streams API](https://developer.mozilla.org/en-US/docs/Web/API/Streams_API/Using_readable_streams)

<br><br><br>

## Niet alles is een chat app

Je hoeft niet altijd een chat applicatie te bouwen. Je kan het LLM ook gebruiken om op andere manieren live content te genereren. In dit voorbeeld genereert de AI een JSON file die we rechtstreeks kunnen afspelen in de `tone.js` music library. Dit kan je gebruiken om live muziekjes te genereren die op dat moment bij je applicatie passen.

#### Prompt
```sh
Can you create a JSON file for use with tone.js, it should contain notes for a chord progression. Do not use any comments or explanation, only return pure JSON
```
#### Result
```json
{
  "chordProgression": [
    {
      "chord": "Cmaj7",
      "notes": ["C4", "E4", "G4", "B4"]
    },
    {
      "chord": "G7",
      "notes": ["G3", "B3", "D4", "F4"]
    },
    {
      "chord": "Am7",
      "notes": ["A3", "C4", "E4", "G4"]
    },
    {
      "chord": "Fmaj7",
      "notes": ["F3", "A3", "C4", "E4"]
    }
  ]
}
```


<br><br><bR>

## Toepassing bedenken

- [Bekijk alvast opdracht 2 voor inspiratie voor een taalmodel toepassing!](https://github.com/HR-CMGT/PRG08-2024-2025/blob/main/opdracht2.md)

<br><br><br>

## Experimenteren met andere providers

- [Mistral](https://js.langchain.com/docs/integrations/chat/mistral/)
- [Anthropic Claude](https://js.langchain.com/docs/integrations/chat/anthropic/)
- [AliBaba Qwen](https://js.langchain.com/docs/integrations/chat/alibaba_tongyi/)
- [All Langchain models](https://js.langchain.com/docs/integrations/chat/)
- [Lokaal LLM](../snippets/local.md)


<br><br><br>

## Links

- [Langchain basics](https://js.langchain.com/docs/tutorials/llm_chain)
- [LangChain quickstart](https://js.langchain.com/docs/get_started/quickstart)
- [Langchain Azure OpenAI](https://js.langchain.com/docs/integrations/chat/azure)
- [Prompt Engineering](https://platform.openai.com/docs/guides/prompt-engineering)
- [Prompt Engineering for Developers - korte cursus](https://www.deeplearning.ai/short-courses/chatgpt-prompt-engineering-for-developers/) 
- [Annuleren van een OpenAI call](https://js.langchain.com/docs/modules/model_io/llms/cancelling_requests)
- [Tokens bijhouden](https://js.langchain.com/docs/modules/model_io/llms/token_usage_tracking)
- [Omgaan met errors](https://js.langchain.com/docs/modules/model_io/llms/dealing_with_api_errors)




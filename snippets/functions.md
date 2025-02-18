# Tool calling

Een taalmodel heeft geen kennis van recent nieuws, sportuitslagen, het weer, of andere hele specifieke recente informatie. Ook is een taalmodel niet altijd goed in berekeningen en kan een taalmodel niet uit zichzelf een game of een smarthome besturen. Zaken waar een taalmodel niet goed in is:

- Berekeningen maken
- Belasting aangifte doen
- Het weerbericht tonen
- Sportuitslagen tonen
- Op google zoeken
- Het licht aan doen (smarthome)
- Dobbelstenen gooien
- Een robot besturen
- CRUD functies (lezen en schrijven naar een database)
- Een grafiek tekenen die echt klopt
- Inloggen op een beveiligde website

Dit kan je toevoegen door je eigen `tools` te schrijven en die toe te voegen aan het taalmodel. Het taalmodel kan nu aan de hand van de prompt zelf bedenken of een van de tools aangeroepen moet worden. In onderstaande voorbeeld schrijven we een functie (tool) die twee nummers vermenigvuldigt. 

<br><br><br>

## Tool 

Het verschil tussen een gewone `javascript function` en een `tool function` is dat je een *schema* moet meegeven waar precies in staat welke variabelen verwacht worden in de functie. 

```js
import { ChatOpenAI } from "@langchain/openai"
import { HumanMessage, AIMessage, ToolMessage, SystemMessage } from "@langchain/core/messages";
import { tool } from "@langchain/core/tools";

const multiplyFunction = ({ a, b }) => a * b;

const multiply = tool(multiplyFunction, {
    name: "multiply",
    description: "Multiply two numbers",
    schema: {
        type: "object",
        properties: {
            a: { type: "number" },
            b: { type: "number" },
        },
        required: ["a", "b"],
    },
});

// test of de functie werkt met "invoke"
let resultA = await multiply.invoke({ a: 3, b: 4 });
console.log(resultA)
```
<br>
<br>
<br>

## Functie aan model koppelen

Via `bindTools` kan je aan het model duidelijk maken welke tools je geschreven hebt, je kan meerdere tools tegelijk doorgeven.

```js
const model = new ChatOpenAI({
    temperature:0.5,
    azureOpenAIApiKey: process.env.AZURE_OPENAI_API_KEY,
    azureOpenAIApiVersion: process.env.OPENAI_API_VERSION,
    azureOpenAIApiInstanceName: process.env.INSTANCE_NAME,
    azureOpenAIApiDeploymentName: process.env.ENGINE_NAME,
})
const modelWithTools = model.bindTools([multiply]);
```
<br>
<br>
<br>

## Taalmodel aanroepen

Het model bepaalt zelf wanneer de tool aangeroepen moet worden, dit kan je als volgt testen:

```js
const messages = [new HumanMessage("What is 3 * 12??")]
const result = await modelWithTools.invoke(messages)
messages.push(result)
```
<br><br><br>

## Tool calls uitvoeren

Als het model vindt dat er tool calls uitgevoerd moeten worden dan zal de `content` leeg zijn, en de `tool_calls` array gevuld zijn. Dit kan je checken in de console. Als dit het geval is moet je via `invoke` zelf de tools uitvoeren:

```js
console.log("I want to call the following tools")
console.log(result.tool_calls)

const toolsByName = {multiply: multiply};
for (const toolCall of result.tool_calls) {
    const selectedTool = toolsByName[toolCall.name];
    console.log("now trying to call " + toolCall.name);
    const toolMessage = await selectedTool.invoke(toolCall);
    messages.push(toolMessage);
}
```

<br><br><br>


## Eindresultaat

Het resultaat van de tool calls zit nu ook in de `messages` array. Dit kan je zien met `console.log(messages)`. Om hier weer een *human readable* bericht van te maken geven we de hele array nog een keer aan langchain:

```js
const endresult = await llmWithTools.invoke(messages);
console.log(endresult.content);
```


<br><br><br>

## Tips

Let op dat je alle resultaten steeds in de `messages` array pushed. Dit zorgt ervoor dat langchain je prompts en tool results automatisch in het juiste formaat zet voor het taalmodel.

Je kan ook beginnen met system instructions om nog duidelijker aan het taalmodel uit te leggen dat er functies gebruikt kunnen worden.

```js
const messages = [
    new SystemMessage("You are a happy little weather assistant. You can use the fetchWeather tool to get the current weather data for a specific location."),
    new HumanMessage("What is the weather in Tokyo?")
]
const result = await modelWithTools.invoke(messages);
```
<br><br><br>

## Ideëen voor applicaties

- Je haalt data op van een externe API (inspiratie: https://apilist.fun)
- Je logt in op een beveiligde website (dit kan je eigen CRUD applicatie zijn)
- Je stuurt een externe applicatie aan (bv. een app van https://ifttt.com, je raspberry pi smarthome, of je eigen javascript game)
- Je doet een zoekopdracht op de LLM search engine https://tavily.com
- weerdata ophalen, kledingadvies geven (mag ik een afritsbroek aan?)
- zeg tegen je [fotolijstje](https://www.youtube.com/watch?v=L5PvQj1vfC4) dat je vandaag een foto van een kat met een piratenhoedje wil zien.


<br><br><br>

## Links

- [Building Langchain tools](https://js.langchain.com/docs/concepts/tools/)
- [Calling Langchain tools](https://js.langchain.com/docs/concepts/tool_calling/)
- [OpenAI detailed info on function calling](https://platform.openai.com/docs/guides/function-calling?lang=node.js)

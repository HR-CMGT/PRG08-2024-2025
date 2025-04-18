# Tool calling

Een taalmodel heeft geen kennis van recent nieuws, sportuitslagen, het weer, of andere hele specifieke recente informatie. Ook is een taalmodel niet altijd goed in berekeningen en kan een taalmodel niet uit zichzelf een game of een smarthome besturen. Zaken waar een taalmodel niet goed in is:

- Berekeningen maken
- Een email sturen
- Een afspraak in google calendar plaatsen
- Een nummer afspelen op spotify
- Een zoekopdracht doen op internet
- Belasting aangifte doen
- Het weerbericht tonen
- Sportuitslagen tonen
- Op google zoeken
- Het licht aan doen (smarthome)
- Dobbelstenen gooien
- Een afbeelding tonen op je [interactieve fotolijstje](https://www.youtube.com/watch?v=L5PvQj1vfC4)
- Een robot besturen
- CRUD functies (lezen en schrijven naar een database)
- Een grafiek tekenen die echt klopt
- Inloggen op een beveiligde website
- Een functie aanroepen op IFTTT

Dit kan je toevoegen door je eigen `tools` te schrijven en die toe te voegen aan het taalmodel. Het taalmodel kan nu aan de hand van de prompt zelf bedenken of een van de tools aangeroepen moet worden. In onderstaande voorbeeld schrijven we een functie (tool) die twee nummers vermenigvuldigt. 

<br><br><br>

## Function

De functie die door het taalmodel aangeroepen kan worden, is een gewone javascript functie. Let op dat eventuele arguments in een object binnenkomen:

```js
const multiplyFunction = ({ a, b }) => a * b;
```

<br><br><br>

## Tool 

Je kan hier nu een tool van maken zodat het taalmodel de functie snapt:

```js
import { AzureChatOpenAI } from "@langchain/openai"
import { HumanMessage, AIMessage, ToolMessage, SystemMessage } from "@langchain/core/messages";
import { tool } from "@langchain/core/tools";

const multiply = tool(multiplyFunction, {
    name: "multiply",                       // een naam die voor het taalmodel duidelijk maakt wat de tool doet
    description: "Multiply two numbers",    // beschrijf wat de tool doet en wanneer het taalmodel dit nodig heeft
    schema: {                               // geef aan welke arguments de tool verwacht. voor "multiply" zijn dat twee getallen met de namen a en b
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
<Br><Br><br>

### Chat History

De chat history is erg belangrijk by tool calling, omdat het taalmodel ook de resultaten van toolcalls moet bijhouden. In de system prompt kan je ook nog wat meer uitleg geven over de tools. In dit voorbeeld gebruiken we de `roles` uit langchain:  `SystemMessage`, `AIMessage`, `HumanMessage`, en daar is nu een nieuwe rol bij gekomen: `ToolMessage`.

```js
const messages = [new SystemMessage("You are a helpful assistant, you can use the multiply tool to multiply two numbers")]
```

<br>
<br>
<br>

## Functie aan model koppelen

Via `bindTools` kan je aan het model duidelijk maken welke tools je geschreven hebt, je kan meerdere tools tegelijk doorgeven.

```js
const model = new AzureChatOpenAI({
    temperature:0.2
}).bindTools([multiply])
```
<br>
<br>
<br>

## Taalmodel aanroepen

Het model bepaalt zelf wanneer de tool aangeroepen moet worden, dit kan je als volgt testen:

```js
messages.push(new HumanMessage("What is 3 * 12?"))
const result = await model.invoke(messages)
messages.push(result)

console.log("I want to call the following tools")
console.log(result.tool_calls)
```
<br><br><br>

## Tool calls uitvoeren

Als het model vindt dat er tool calls uitgevoerd moeten worden dan zal de `content` leeg zijn, en de `tool_calls` array gevuld zijn. Dit moet je handmatig checken met een `for` loop. 

Als er `tool_calls` in de response zitten, dan moet je die via `invoke` zelf uitvoeren:

```js
const tools = [multiply]; 
const toolsByName = Object.fromEntries(tools.map(tool => [tool.name, tool]));

for (const toolCall of result.tool_calls) {
    const selectedTool = toolsByName[toolCall.name];
    console.log("now trying to call " + toolCall.name);
    const toolMessage = await selectedTool.invoke(toolCall);
    messages.push(toolMessage);
}
```

<br><br><br>


## Eindresultaat

Als er *geen* tool calls waren, dan is je `result` gewoon het antwoord van het taalmodel dat je zonder tools ook zou krijgen. Als er *wel* tool calls gedaan zijn, dan moet je het resultaat daarvan nog een keer naar het taalmodel sturen, om het *human readable* te maken.

```js
if (response.tool_calls.length > 0) {
    response = await model.invoke(messages);
    console.log(response.content);
}
```


<br><br><br>

## Tips

- Let op dat je na elke `let result = await model.invoke(...)` het resultaat toevoegt aan de chat history met `messages.push(result)` !

<br><br><br>



# Tavily voorbeeld

[Tavily](https://tavily.com) is een web search engine waarmee het taalmodel kan "googlen". 

```sh
npm install @tavily/core
```

#### Tavily function

```js
import { tavily } from "@tavily/core";

const client = tavily({ apiKey: process.env.TAVILY_KEY });
const options = {topic: "news", timeRange: "m", includeAnswer: "true", maxResults:1, days: 5 }

export async function searchTavily({query}) {   
    try {
        const response = await client.search(query, options);
        const result = response.results[0].title + " " + response.results[0].content
        console.log(result);
        return result
    } catch (error) {
        console.error(error);
    }
}
```
#### Tool bouwen

```js
const newsTool = tool(searchTavily, {
    name: "searchTavily",
    description: "Fetches any kind of recent news about a subject, from an online search engine.", 
    schema: {
        type: "object",
        properties: {
            subject: { type: "string" },
        },
        required: ["subject"],
    },
});

const model = new AzureChatOpenAI({
    temperature:0.2
}).bindTools([newsTool])
```



<br><br><br>

## Links

- [Een tool definiëren](https://js.langchain.com/docs/concepts/tools/)
- [Een tool automatisch laten uitvoeren door het taalmodel](https://js.langchain.com/docs/concepts/tool_calling/)
- [OpenAI LangChain Function Calling](https://js.langchain.com/docs/integrations/chat/openai)
- [OpenAI Agents in Langchain](https://js.langchain.com/docs/modules/agents/)
- [OpenAI detailed info on function calling](https://platform.openai.com/docs/guides/function-calling?lang=node.js)

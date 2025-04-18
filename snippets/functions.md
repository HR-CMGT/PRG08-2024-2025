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

## Tool 

Het verschil tussen een gewone `javascript function` en een `tool function` is dat je een *schema* moet meegeven waar precies in staat welke variabelen verwacht worden in de functie. 

```js
import { AzureChatOpenAI } from "@langchain/openai"
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
const messages = [new HumanMessage("What is 3 * 12??")]
const result = await modelWithTools.invoke(messages)
messages.push(result)
```
<br><br><br>

## Tool calls uitvoeren

Als het model vindt dat er tool calls uitgevoerd moeten worden dan zal de `content` leeg zijn, en de `tool_calls` array gevuld zijn. Dit moet je handmatig checken met een `if` statement of een `for` loop. Als er `tool_calls` in de response zitten, dan moet je die via `invoke` zelf uitvoeren:

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
const endresult = await modelWithTools.invoke(messages);
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


### Tavily voorbeeld

[Tavily](https://tavily.com) is een web search engine waarmee het taalmodel kan "googlen". 

#### Tavily Instellen

```js
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
const newsTool = tool(searchTavily,{
        name: "searchTavily",
        description: "Fetches any kind of recent news from an online search engine. This is recent news that the language model would otherwise not know about.", // helps the llm to understand when to use the tool
        schema: z.object({
            query: z.string().describe("The subject that the news should be about."), // helps the llm to format the input
        }),
    }
);

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

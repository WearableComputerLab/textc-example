using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Takenet.Textc;
using Takenet.Textc.Csdl;
using Takenet.Textc.Processors;
using UnityEngine;
using UnityEngine.Events;


public class ConversationalTest : MonoBehaviour
{
    [SerializeField]
    UnityEvent<int, int> addEvent;

    private Syntax sumSyntax = CsdlParser.Parse("operation+:Word(sum) a:Integer :Word?(and) b:Integer");

    private Syntax chart2dSyntax = CsdlParser.Parse(":Word(show) :Word?(me) dim1:Word() dim2:Word()");


    Func<int, int, Task<int>> sumFunc = (a, b) => Task.FromResult(5);
    Func<string, string, Task<string>> sumFunc2 = (a, b) => Task.FromResult("hat");


    // Define a output processor that prints the command results to the console
    DelegateOutputProcessor<int> outputProcessor = new DelegateOutputProcessor<int>((o, context) => print($"Result: {o}"));

    // Start is called before the first frame update
    async void Start()
    {
        try
        {
            var test = await MyTest();
        }
        catch
        {
            Debug.Log("An error occurred");
        }

    }

    async Task<int> MyTest()
    {

        var sumCommandProcessor = new DelegateCommandProcessor(
            sumFunc,
            true,
            outputProcessor,
            sumSyntax
        );

        var chart2DCommandProcessor = new DelegateCommandProcessor(
            sumFunc2,
            true,
            null,
            chart2dSyntax
        );

        var textProcessor = new TextProcessor();
        textProcessor.CommandProcessors.Add(sumCommandProcessor);

        var context = new RequestContext();

        try
        {
            await textProcessor.ProcessAsync("sum 3 and 5", context, CancellationToken.None);
        }
        catch (MatchNotFoundException)
        {
            print("There's no match for the specified input");
        }
        return 1;
    }

    int DoSum(int a, int b)
    {
        return 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

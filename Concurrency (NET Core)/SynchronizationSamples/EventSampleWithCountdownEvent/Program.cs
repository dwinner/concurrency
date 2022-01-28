using System;
using System.Threading;
using System.Threading.Tasks;

const int taskCount = 4;

CountdownEvent cEvent = new(taskCount);
Calculator[] calcs = new Calculator[taskCount];

for (var i = 0; i < taskCount; i++)
{
   calcs[i] = new Calculator(cEvent);
   var i1 = i;
   Task.Run(() => calcs[i1].Calculation(i1 + 1, i1 + 3));
}

cEvent.Wait();
Console.WriteLine("all finished");

for (var i = 0; i < taskCount; i++)
{
   Console.WriteLine($"task for {i}, result: {calcs[i].Result}");
}

cEvent.Dispose();
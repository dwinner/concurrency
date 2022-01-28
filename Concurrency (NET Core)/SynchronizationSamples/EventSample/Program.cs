using System;
using System.Threading;
using System.Threading.Tasks;

const int taskCount = 4;

ManualResetEventSlim[] mEvents = new ManualResetEventSlim[taskCount];
WaitHandle[] waitHandles = new WaitHandle[taskCount];
Calculator[] calcs = new Calculator[taskCount];

for (var i = 0; i < taskCount; i++)
{
   var i1 = i;
   mEvents[i] = new ManualResetEventSlim(false);
   waitHandles[i] = mEvents[i].WaitHandle;
   calcs[i] = new Calculator(mEvents[i]);
   Task.Run(() => calcs[i1].Calculation(i1 + 1, i1 + 3));
}

for (var i = 0; i < taskCount; i++)
{
   //   int index = WaitHandle.WaitAny(mEvents.Select(e => e.WaitHandle).ToArray());
   var index = WaitHandle.WaitAny(waitHandles);
   if (index == WaitHandle.WaitTimeout)
   {
      Console.WriteLine("Timeout!!");
   }
   else
   {
      mEvents[index].Reset();
      Console.WriteLine($"finished task for {index}, result: {calcs[index].Result}");
   }
}

for (var i = 0; i < taskCount; i++)
{
   mEvents[i].Dispose();
   waitHandles[i].Dispose();
}
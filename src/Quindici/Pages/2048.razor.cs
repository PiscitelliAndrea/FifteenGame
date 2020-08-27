using G2048.Data;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading;

namespace G2048.Pages
{
    public partial class G2048Game : ComponentBase
    {
        #region Injection

        [Inject]
        protected NumbersGenerator Numbers { get; set; }

        #endregion

        #region Internal properties

        protected Timer timer { get; set; }
        protected bool TimerStarted = false;

        protected List<Number> numbers;
        protected Number number;

        //private int ElapsedTime { get; set; }
        protected TimeSpan ElapsedTime = TimeSpan.FromMilliseconds(0);

        #endregion

        #region Life Cycle events

        protected override void OnInitialized() => numbers = Numbers.GenerateNumbers();

        #endregion

        #region Numbers Methods

        protected void ClickNumber(int direzione)
        {
            if (!Numbers.Done)
            {
                if (!TimerStarted)
                {
                    TimerStarted = true;
                    StartCounter();
                }

                numbers = Numbers.TryMoveNumber(direzione);
                if (Numbers.Done)
                    StopCounter();
                this.StateHasChanged();
            }
        }

        protected void Restart()
        {
            ResetCounter();
            TimerStarted = true;
            StartCounter();
            numbers = Numbers.Restart();
            this.StateHasChanged();
        }

        #endregion

        #region Timer Methods

        void StartCounter()
        {
            timer = new Timer(new TimerCallback(_ =>
            {
                //ElapsedTime++;
                ElapsedTime = ElapsedTime.Add(new TimeSpan(0, 0, 1));

                // Note that the following line is necessary because otherwise
                // Blazor would not recognize the state change and not refresh the UI
                InvokeAsync(() =>
                {
                    StateHasChanged();
                });
            }), null, 1000, 1000);
        }

        void ResetCounter()
        {
            ElapsedTime = new TimeSpan(0, 0, 0);
        }

        void StopCounter()
        {
            timer.Dispose();
        }

        #endregion
    }
}
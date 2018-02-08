using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartFreezeFA.Models;
using SmartFreezeFA.Services;

namespace SmartFreezeFA.Services
{
    public class LowBatteryService
    {
        const double maxVoltageValue = 3.6;
        private readonly AlarmService alarmService;

        public LowBatteryService(AlarmService alarmService)
        {
            this.alarmService = alarmService;
        }

        public void CheckBatteryLevel(Telemetry telemetry)
        {
            Alarm alarm = null;
            string ShortDescription, Description = null;
                if (telemetry.BatteryVoltage <= (maxVoltageValue * 0.15))
                {
                    //créer alarme level 3 15%
                    ShortDescription = "batterie < 15%";
                    Description = "Batterie très faible pour le capteur (moins de 15%)";
                    alarm = alarmService.CreateAlarm(telemetry.DeviceId, null, Alarm.Type.BatteryWarning, Alarm.Gravity.Critical, ShortDescription, Description);
                }
                else if (telemetry.BatteryVoltage <= (maxVoltageValue * 0.3))
                {
                    //créer alarme level 2 30%
                    ShortDescription = "batterie < 30%";
                    Description = "Batterie faible pour le capteur (moins de 30%)";
                    alarm = alarmService.CreateAlarm(telemetry.DeviceId, null, Alarm.Type.BatteryWarning, Alarm.Gravity.Critical, ShortDescription, Description);

                }
                else if (telemetry.BatteryVoltage <= (maxVoltageValue * 0.5))
                {
                    //créer alarme level 1 50%
                    ShortDescription = "batterie < 50%";
                    Description = "Batterie à 50% pour le capteur";
                    alarm = alarmService.CreateAlarm(telemetry.DeviceId, null, Alarm.Type.BatteryWarning, Alarm.Gravity.Critical, ShortDescription, Description);

                }
        }
    }
}

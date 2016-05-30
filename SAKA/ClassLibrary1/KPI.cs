using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAKA.DTO;

namespace SAKA.Bussiness
{
    public class Kpi
    {
        public static ScoreCard[] GetScorecard()
        {
            using (var dc = new SAKADataDataContext())
            {
                var listKpi = dc.KPIs.Where(kpi => kpi.KPI_Values.Any()).Select(c => new
                {
                    c.ID,
                    c.Name,
                    c.Period,
                    c.Unit,
                    c.Target,
                    c.Threshold,
                    c.ThresholdType,
                    c.Direction
                }).ToList();

                var listDTO = new List<ScoreCard>();

                foreach (var kpi in listKpi)
                {
                    var value = dc.KPI_Values.Where(c => c.KPI_ID == kpi.ID).OrderByDescending(c => c.Date).Select(c => new { c.Value, c.Date }).First();

                    var item = new ScoreCard();

                    item.NAME = kpi.Name;
                    item.UNIT = kpi.Unit;
                    item.DATE = value.Date;
                    item.VALUE = value.Value;
                    item.PERIOD = (Period)kpi.Period;
                    item.STATU = Kpi.CalculateStatu(kpi.Threshold, kpi.ThresholdType, kpi.Direction.Value, kpi.Target, value.Value);

                    listDTO.Add(item);
                }

                return listDTO.ToArray();
            }
        }
        public static DTO_Gauge[] GetGauge()
        {
            using (var dc = new SAKADataDataContext())
            {
                var listKpi = dc.KPIs.Where(c => c.KPI_Values.Any()).Select(c => new
                {
                    c.Name,
                    c.ID,
                    c.Threshold,
                    c.ThresholdType,
                    c.Direction,
                    c.Target,
                    c.Unit
                });

                var listGauge = new List<DTO_Gauge>();
                foreach (var dto in listKpi)
                {
                    var value = dc.KPI_Values.Where(c => c.KPI_ID == dto.ID).Select(c => new { c.Value, c.Date }).OrderByDescending(c => c.Date).First();
                    var item = new DTO_Gauge();
                    var sapma = dto.ThresholdType ? dto.Threshold : dto.Target * dto.Threshold / 100;

                    item.NAME = dto.Name;
                    item.UNIT = dto.Unit;
                    item.VALUE = value.Value;
                    item.DIRECTION = dto.Direction == true ? Direction.positive : Direction.negative;
                    item.TARGET_MAX = dto.Target + sapma;
                    item.TARGET_MIN = dto.Target - sapma;

                    listGauge.Add(item);
                }
                return listGauge.ToArray();
            }
        }
        private static Statu CalculateStatu(decimal threshold, bool thresholdType, bool direction, decimal target, decimal value)
        {
            var sapma = thresholdType ? target * threshold / 100 : threshold;

            if (target + sapma < value)
            {
                return direction ? Statu.Good : Statu.Bad;
            }

            if (target - sapma > value)
            {
                return direction ? Statu.Bad : Statu.Good;
            }

            return Statu.Notr;
        }
    }
}


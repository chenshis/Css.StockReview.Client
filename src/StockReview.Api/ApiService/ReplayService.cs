using Newtonsoft.Json;
using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
using StockReview.Infrastructure.Config;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using static StockReview.Api.Dtos.SharesBasicDataDto;

namespace StockReview.Api.ApiService
{

    public class ReplayService : IReplayService
    {
        private readonly HttpClient _stockHttpClient;

        public ReplayService(IHttpClientFactory httpClientFactory)
        {
            _stockHttpClient = httpClientFactory.CreateClient();
        }

        public List<LeadingDateHeaderDto> GetLeadingGroupPromotion(DateTime date)
        {
            var leadingList = new List<LeadingDateHeaderDto>();

            string[] days = { "周日", "周一", "周二", "周三", "周四", "周五", "周六" };

            int workDayToAdd = 20;
            int workDaysCount = 1;

            #region 左侧菜单
            leadingList.Add(new LeadingDateHeaderDto
            {
                HeadRow = 0,
                HeadColumn = 0,
                HeadName = "版数"
            });

            leadingList.Add(new LeadingDateHeaderDto
            {
                HeadRow = 3,
                HeadColumn = 0,
                HeadName = string.Format("13板太"),
            });
            leadingList.Add(new LeadingDateHeaderDto
            {
                HeadRow = 4,
                HeadColumn = 0,
                HeadName = string.Format("12板月"),
            });
            leadingList.Add(new LeadingDateHeaderDto
            {
                HeadRow = 5,
                HeadColumn = 0,
                HeadName = string.Format("11板量"),
            });
            leadingList.Add(new LeadingDateHeaderDto
            {
                HeadRow = 6,
                HeadColumn = 0,
                HeadName = string.Format("10板天"),
            });
            leadingList.Add(new LeadingDateHeaderDto
            {
                HeadRow = 7,
                HeadColumn = 0,
                HeadName = string.Format("9板飞"),
            });
            leadingList.Add(new LeadingDateHeaderDto
            {
                HeadRow = 8,
                HeadColumn = 0,
                HeadName = string.Format("8板仙"),
            });
            leadingList.Add(new LeadingDateHeaderDto
            {
                HeadRow = 9,
                HeadColumn = 0,
                HeadName = string.Format("7板神"),
            });
            leadingList.Add(new LeadingDateHeaderDto
            {
                HeadRow = 10,
                HeadColumn = 0,
                HeadName = string.Format("6板鬼"),
            });
            leadingList.Add(new LeadingDateHeaderDto
            {
                HeadRow = 11,
                HeadColumn = 0,
                HeadName = string.Format("5板魔"),
            });
            leadingList.Add(new LeadingDateHeaderDto
            {
                HeadRow = 12,
                HeadColumn = 0,
                HeadName = string.Format("4板怪"),
            });
            leadingList.Add(new LeadingDateHeaderDto
            {
                HeadRow = 13,
                HeadColumn = 0,
                HeadName = string.Format("3板妖"),
            });
            leadingList.Add(new LeadingDateHeaderDto
            {
                HeadRow = 14,
                HeadColumn = 0,
                HeadName = string.Format("2板龙"),
            });
            leadingList.Add(new LeadingDateHeaderDto
            {
                HeadRow = 15,
                HeadColumn = 0,
                HeadName = string.Format("首板"),
            });
            #endregion

            while (workDaysCount <= workDayToAdd)
            {
                DayOfWeek dayOfWeek = date.DayOfWeek;

                if (dayOfWeek != DayOfWeek.Saturday && dayOfWeek != DayOfWeek.Sunday)
                {
                    var url = SystemConstantTwo.LeadingGroupPromotionDataUrl + date.ToString("yyyyMMdd");
                    var response = _stockHttpClient.GetAsync(url).Result;
                    var content = response.Content.ReadAsStringAsync().Result;

                    var headRow6 = 1;
                    var headRow7 = 1;
                    var headRow8 = 1;
                    var headRow9 = 1;
                    var headRow10 = 1;
                    var headRow11 = 1;
                    var headRow12 = 1;
                    var headRow13 = 1;

                    if (!string.IsNullOrEmpty(content))
                    {
                        SharesBasicDataDto.Root root = JsonConvert.DeserializeObject<SharesBasicDataDto.Root>(content);

                        leadingList.Add(new LeadingDateHeaderDto
                        {
                            HeadRow = 0,
                            HeadColumn = workDaysCount,
                            HeadName = string.Format("{0}\n{1}\n{2}", date.Month + "月" + date.Day + "日", days[(int)dayOfWeek], workDaysCount)
                        });

                        if (root.data.more.limit_up_list.Count > 0)
                        {
                            var moreStringBuilder = new StringBuilder();
                            for (int i = 0; i < root.data.more.limit_up_list.Count; i++)
                            {
                                moreStringBuilder.AppendLine(i + 1 + root.data.more.limit_up_list[i].limit_up_reason);

                                #region 判断是否大于5
                                var moreUrl = SystemConstantTwo.LeadingGroupPromotionDataUrl + date.AddDays(-1).ToString("yyyyMMdd");
                                var responseMore = _stockHttpClient.GetAsync(moreUrl).Result;
                                var contentMore = responseMore.Content.ReadAsStringAsync().Result;

                                if (!string.IsNullOrEmpty(contentMore))
                                {
                                    SharesBasicDataDto.Root rootFifth = JsonConvert.DeserializeObject<SharesBasicDataDto.Root>(content);

                                    var rootFifthInfo = rootFifth.data.fifth.limit_up_list.Where(x => x.stockCode.Equals(root.data.more.limit_up_list[i].stockCode)).FirstOrDefault();
                                    var rootFourthInfo = rootFifth.data.fourth.limit_up_list.Where(x => x.stockCode.Equals(root.data.more.limit_up_list[i].stockCode)).FirstOrDefault();
                                    var rootThirdInfo = rootFifth.data.third.limit_up_list.Where(x => x.stockCode.Equals(root.data.more.limit_up_list[i].stockCode)).FirstOrDefault();
                                    var rootSecondInfo = rootFifth.data.second.limit_up_list.Where(x => x.stockCode.Equals(root.data.more.limit_up_list[i].stockCode)).FirstOrDefault();
                                    var rootFirstInfo = rootFifth.data.first.limit_up_list.Where(x => x.stockCode.Equals(root.data.more.limit_up_list[i].stockCode)).FirstOrDefault();
                                    var rootMoveInfo = rootFifth.data.more.limit_up_list.Where(x => x.stockCode.Equals(root.data.more.limit_up_list[i].stockCode)).FirstOrDefault();

                                    if (rootFirstInfo != null)
                                    {

                                        var firstInfo = leadingList.Where(x => x.HeadColumn == workDaysCount && x.HeadRow == 10).FirstOrDefault();
                                        var stringBuilder = new StringBuilder();
                                        if (firstInfo != null)
                                        {
                                            stringBuilder.AppendLine(firstInfo.HeadName);
                                            leadingList.Remove(firstInfo);
                                        }
                                        stringBuilder.AppendLine(headRow6 + root.data.more.limit_up_list[i].stockName);

                                        leadingList.Add(new LeadingDateHeaderDto
                                        {
                                            HeadRow = 10,
                                            HeadColumn = workDaysCount,
                                            HeadName = stringBuilder.ToString()
                                        });
                                        headRow6 = ++headRow6;
                                    }
                                    if (rootSecondInfo != null)
                                    {
                                        var secondInfo = leadingList.Where(x => x.HeadColumn == workDaysCount && x.HeadRow == 9).FirstOrDefault();

                                        var stringBuilder = new StringBuilder();
                                        if (secondInfo != null)
                                        {
                                            stringBuilder.AppendLine(secondInfo.HeadName);
                                            leadingList.Remove(secondInfo);
                                        }
                                        stringBuilder.AppendLine(headRow7 + root.data.more.limit_up_list[i].stockName);

                                        leadingList.Add(new LeadingDateHeaderDto
                                        {
                                            HeadRow = 9,
                                            HeadColumn = workDaysCount,
                                            HeadName = stringBuilder.ToString()
                                        });
                                        headRow7 = ++headRow7;
                                    }
                                    if (rootThirdInfo != null)
                                    {
                                        var thirdInfo = leadingList.Where(x => x.HeadColumn == workDaysCount && x.HeadRow == 8).FirstOrDefault();

                                        var stringBuilder = new StringBuilder();
                                        if (thirdInfo != null)
                                        {
                                            stringBuilder.AppendLine(thirdInfo.HeadName);
                                            leadingList.Remove(thirdInfo);
                                        }
                                        stringBuilder.AppendLine(headRow8 + root.data.more.limit_up_list[i].stockName);

                                        leadingList.Add(new LeadingDateHeaderDto
                                        {
                                            HeadRow = 8,
                                            HeadColumn = workDaysCount,
                                            HeadName = stringBuilder.ToString()
                                        });
                                        headRow8 = ++headRow8;
                                    }
                                    if (rootFourthInfo != null)
                                    {
                                        var fourthInfo = leadingList.Where(x => x.HeadColumn == workDaysCount && x.HeadRow == 7).FirstOrDefault();

                                        var stringBuilder = new StringBuilder();
                                        if (fourthInfo != null)
                                        {
                                            stringBuilder.AppendLine(fourthInfo.HeadName);
                                            leadingList.Remove(fourthInfo);
                                        }
                                        stringBuilder.AppendLine(headRow9 + root.data.more.limit_up_list[i].stockName);

                                        leadingList.Add(new LeadingDateHeaderDto
                                        {
                                            HeadRow = 7,
                                            HeadColumn = workDaysCount,
                                            HeadName = stringBuilder.ToString()
                                        });
                                        headRow9 = ++headRow9;
                                    }
                                    if (rootFifthInfo != null)
                                    {
                                        var fifthInfo = leadingList.Where(x => x.HeadColumn == workDaysCount && x.HeadRow == 6).FirstOrDefault();

                                        var stringBuilder = new StringBuilder();
                                        if (fifthInfo != null)
                                        {
                                            stringBuilder.AppendLine(fifthInfo.HeadName);
                                            leadingList.Remove(fifthInfo);
                                        }
                                        stringBuilder.AppendLine(headRow10 + root.data.more.limit_up_list[i].stockName);

                                        leadingList.Add(new LeadingDateHeaderDto
                                        {
                                            HeadRow = 6,
                                            HeadColumn = workDaysCount,
                                            HeadName = stringBuilder.ToString()
                                        });
                                        headRow10 = ++headRow10;
                                    }

                                    if (rootMoveInfo != null)
                                    {
                                        #region 判断是否大于5
                                        var moreTwoUrl = SystemConstantTwo.LeadingGroupPromotionDataUrl + date.AddDays(-2).ToString("yyyyMMdd");
                                        var responseMoreTwo = _stockHttpClient.GetAsync(moreTwoUrl).Result;
                                        var contentMoreTwo = responseMoreTwo.Content.ReadAsStringAsync().Result;

                                        if (!string.IsNullOrEmpty(contentMoreTwo))
                                        {
                                            SharesBasicDataDto.Root rootTwo = JsonConvert.DeserializeObject<SharesBasicDataDto.Root>(content);

                                            var rootFifthTwoInfo = rootTwo.data.fifth.limit_up_list.Where(x => x.stockCode.Equals(root.data.more.limit_up_list[i].stockCode)).FirstOrDefault();
                                            var rootFourthTwoInfo = rootTwo.data.fourth.limit_up_list.Where(x => x.stockCode.Equals(root.data.more.limit_up_list[i].stockCode)).FirstOrDefault();
                                            var rootThirdTwoInfo = rootTwo.data.third.limit_up_list.Where(x => x.stockCode.Equals(root.data.more.limit_up_list[i].stockCode)).FirstOrDefault();
                                            var rootSecondTwoInfo = rootTwo.data.second.limit_up_list.Where(x => x.stockCode.Equals(root.data.more.limit_up_list[i].stockCode)).FirstOrDefault();
                                            var rootFirstTwoInfo = rootTwo.data.first.limit_up_list.Where(x => x.stockCode.Equals(root.data.more.limit_up_list[i].stockCode)).FirstOrDefault();
                                            var rootMoveTwoInfo = rootTwo.data.more.limit_up_list.Where(x => x.stockCode.Equals(root.data.more.limit_up_list[i].stockCode)).FirstOrDefault();

                                            if (rootFirstTwoInfo != null)
                                            {
                                                var firstInfoTwo = leadingList.Where(x => x.HeadColumn == workDaysCount && x.HeadRow == 5).FirstOrDefault();

                                                var stringBuilder = new StringBuilder();
                                                if (firstInfoTwo != null)
                                                {
                                                    stringBuilder.AppendLine(firstInfoTwo.HeadName);
                                                    leadingList.Remove(firstInfoTwo);
                                                }
                                                stringBuilder.AppendLine(headRow11 + root.data.more.limit_up_list[i].stockName);

                                                leadingList.Add(new LeadingDateHeaderDto
                                                {
                                                    HeadRow = 5,
                                                    HeadColumn = workDaysCount,
                                                    HeadName = stringBuilder.ToString()
                                                });
                                                headRow11 = ++headRow11;
                                            }
                                            if (rootSecondTwoInfo != null)
                                            {
                                                var secondInfoTwo = leadingList.Where(x => x.HeadColumn == workDaysCount && x.HeadRow == 4).FirstOrDefault(); ;

                                                var stringBuilder = new StringBuilder();
                                                if (secondInfoTwo != null)
                                                {
                                                    stringBuilder.AppendLine(secondInfoTwo.HeadName);
                                                    leadingList.Remove(secondInfoTwo);
                                                }
                                                stringBuilder.AppendLine(headRow12 + root.data.more.limit_up_list[i].stockName);

                                                leadingList.Add(new LeadingDateHeaderDto
                                                {
                                                    HeadRow = 4,
                                                    HeadColumn = workDaysCount,
                                                    HeadName = stringBuilder.ToString()
                                                });
                                                headRow12 = ++headRow12;
                                            }
                                            if (rootThirdTwoInfo != null || rootFourthTwoInfo != null || rootFifthTwoInfo != null || rootMoveTwoInfo != null)
                                            {
                                                var thirdInfoTwo = leadingList.Where(x => x.HeadColumn == workDaysCount && x.HeadRow == 3).FirstOrDefault();

                                                var stringBuilder = new StringBuilder();
                                                if (thirdInfoTwo != null)
                                                {
                                                    stringBuilder.AppendLine(thirdInfoTwo.HeadName);
                                                    leadingList.Remove(thirdInfoTwo);
                                                }
                                                stringBuilder.AppendLine(headRow13 + root.data.more.limit_up_list[i].stockName);

                                                leadingList.Add(new LeadingDateHeaderDto
                                                {
                                                    HeadRow = 3,
                                                    HeadColumn = workDaysCount,
                                                    HeadName = stringBuilder.ToString()
                                                });
                                                headRow13 = ++headRow13;
                                            }
                                        }
                                        #endregion
                                    }
                                }
                                #endregion
                            }

                            leadingList.Add(new LeadingDateHeaderDto
                            {
                                HeadRow = 1,
                                HeadColumn = workDaysCount,
                                HeadName = moreStringBuilder.ToString()
                            });
                        }

                        if (root.data.first.limit_up_list.Count > 0)
                        {
                            var firstStringBuilder = new StringBuilder();
                            for (int i = 0; i < root.data.first.limit_up_list.Count; i++)
                            {
                                firstStringBuilder.AppendLine(i + 1 + root.data.first.limit_up_list[i].stockName);
                            }

                            leadingList.Add(new LeadingDateHeaderDto
                            {
                                HeadRow = 15,
                                HeadColumn = workDaysCount,
                                HeadName = firstStringBuilder.ToString()
                            });
                        }

                        if (root.data.second.limit_up_list.Count > 0)
                        {
                            var secondStringBuilder = new StringBuilder();
                            for (int i = 0; i < root.data.second.limit_up_list.Count; i++)
                            {
                                secondStringBuilder.AppendLine((i + 1) + root.data.second.limit_up_list[i].stockName);
                            }

                            leadingList.Add(new LeadingDateHeaderDto
                            {
                                HeadRow = 14,
                                HeadColumn = workDaysCount,
                                HeadName = secondStringBuilder.ToString()
                            });
                        }

                        if (root.data.third.limit_up_list.Count > 0)
                        {
                            var thirdStringBuilder = new StringBuilder();
                            for (int i = 0; i < root.data.third.limit_up_list.Count; i++)
                            {
                                thirdStringBuilder.AppendLine((i + 1) + root.data.third.limit_up_list[i].stockName);
                            }

                            leadingList.Add(new LeadingDateHeaderDto
                            {
                                HeadRow = 13,
                                HeadColumn = workDaysCount,
                                HeadName = thirdStringBuilder.ToString()
                            });
                        }

                        if (root.data.fourth.limit_up_list.Count > 0)
                        {
                            var fourthStringBuilder = new StringBuilder();
                            for (int i = 0; i < root.data.fourth.limit_up_list.Count; i++)
                            {
                                fourthStringBuilder.AppendLine((i + 1) + root.data.fourth.limit_up_list[i].stockName);
                            }

                            leadingList.Add(new LeadingDateHeaderDto
                            {
                                HeadRow = 12,
                                HeadColumn = workDaysCount,
                                HeadName = fourthStringBuilder.ToString()
                            });
                        }

                        if (root.data.fifth.limit_up_list.Count > 0)
                        {
                            var fifthStringBuilder = new StringBuilder();
                            for (int i = 0; i < root.data.fifth.limit_up_list.Count; i++)
                            {
                                fifthStringBuilder.AppendLine((i + 1) + root.data.fifth.limit_up_list[i].stockName);

                            }

                            leadingList.Add(new LeadingDateHeaderDto
                            {
                                HeadRow = 11,
                                HeadColumn = workDaysCount,
                                HeadName = fifthStringBuilder.ToString()
                            });
                        }

                        workDaysCount++;
                    }

                }
                date = date.AddDays(-1);
            }

            return leadingList;
        }

        public MarketLadderDto GetMarketLadder(DateTime date)
        {
            var marketLadderList = new MarketLadderDto()
            {
                MarketTitle = string.Empty,
                MarketLadderLists = new List<MarketLadderList>(),
                MarketLadderNewsLists = new List<MarketLadderNewsList>()
            };

            var url = SystemConstantTwo.LeadingGroupPromotionDataUrl + date.ToString("yyyyMMdd");
            var response = _stockHttpClient.GetAsync(url).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            if (!string.IsNullOrEmpty(content))
            {
                SharesBasicDataDto.Root root = JsonConvert.DeserializeObject<SharesBasicDataDto.Root>(content);
                marketLadderList.MarketTitle = root.data.trade_date + "  涨停" + root.data.total.limit_up_num + "只    晋级率:" + root.data.total.promotion_rate + "%   炸板率:" + root.data.total.plate_frying_rate + "%    竞价涨幅:" + root.data.total.call_auction_rise + "%";
                if (root.data.more.limit_up_num > 0)
                {
                    var marketLadderLists = new MarketLadderList()
                    {
                        MarketLadderInfos = new List<MarketLadderInfo>()
                    };
                    marketLadderLists.MarketLadderBoard = "高度龙头";
                    marketLadderLists.MarketLadderNumber = root.data.more.limit_up_num + "只";
                    marketLadderLists.MarketLadderDescibe = string.Format("(晋级率：{0}%   炸板率：{1}%   竞价涨幅：{2}% )"
                        , root.data.more.promotion_rate, root.data.more.plate_frying_rate, root.data.more.call_auction_rise);

                    var MarketLadderInfoList=new List<MarketLadderInfo>();
                    for (int i = 0; i < root.data.more.limit_up_list.Count; i++)
                    {
                        MarketLadderInfoList.Add(new Dtos.MarketLadderInfo
                        {
                            MarketLadderCode = root.data.more.limit_up_list[i].stockCode,
                            MarketLadderName = root.data.more.limit_up_list[i].stockName,
                            MarketLadderFirstLimitUp = root.data.more.limit_up_list[i].first_limit_up_time,
                            MarketLadderReasonLimitUp = root.data.more.limit_up_list[i].limit_up_reason
                        });
                    }
                    marketLadderLists.MarketLadderInfos.AddRange(MarketLadderInfoList);
                    marketLadderList.MarketLadderLists.Add(marketLadderLists);
                }

                if (root.data.fifth.limit_up_num > 0)
                {
                    var marketLadderListsFifth = new MarketLadderList()
                    {
                        MarketLadderInfos = new List<MarketLadderInfo>()
                    };
                    marketLadderListsFifth.MarketLadderBoard = "五连板";
                    marketLadderListsFifth.MarketLadderNumber = root.data.fifth.limit_up_num + "只";
                    marketLadderListsFifth.MarketLadderDescibe = string.Format("(晋级率：{0}%   炸板率：{1}%   竞价涨幅：{2}% )"
                        , root.data.fifth.promotion_rate, root.data.fifth.plate_frying_rate, root.data.fifth.call_auction_rise);

                    var MarketLadderInfoListFifth = new List<MarketLadderInfo>();
                 
                    for (int i = 0; i < root.data.fifth.limit_up_list.Count; i++)
                    {
                        MarketLadderInfoListFifth.Add(new Dtos.MarketLadderInfo
                        {
                            MarketLadderCode = root.data.fifth.limit_up_list[i].stockCode,
                            MarketLadderName = root.data.fifth.limit_up_list[i].stockName,
                            MarketLadderFirstLimitUp = root.data.fifth.limit_up_list[i].first_limit_up_time,
                            MarketLadderReasonLimitUp = root.data.fifth.limit_up_list[i].limit_up_reason
                        });
                    }
                    marketLadderListsFifth.MarketLadderInfos.AddRange(MarketLadderInfoListFifth);
                    marketLadderList.MarketLadderLists.Add(marketLadderListsFifth);
                }

                if (root.data.fourth.limit_up_num > 0)
                {
                    var marketLadderListsFourth = new MarketLadderList()
                    {
                        MarketLadderInfos = new List<MarketLadderInfo>()
                    };
                    marketLadderListsFourth.MarketLadderBoard = "四连板";
                    marketLadderListsFourth.MarketLadderNumber = root.data.fourth.limit_up_num + "只";
                    marketLadderListsFourth.MarketLadderDescibe = string.Format("(晋级率：{0}%   炸板率：{1}%   竞价涨幅：{2}% )"
                        , root.data.fourth.promotion_rate, root.data.fourth.plate_frying_rate, root.data.fourth.call_auction_rise);

                    var MarketLadderInfoListFourth = new List<MarketLadderInfo>();
                    for (int i = 0; i < root.data.fourth.limit_up_list.Count; i++)
                    {
                        MarketLadderInfoListFourth.Add(new Dtos.MarketLadderInfo
                        {
                            MarketLadderCode = root.data.fourth.limit_up_list[i].stockCode,
                            MarketLadderName = root.data.fourth.limit_up_list[i].stockName,
                            MarketLadderFirstLimitUp = root.data.fourth.limit_up_list[i].first_limit_up_time,
                            MarketLadderReasonLimitUp = root.data.fourth.limit_up_list[i].limit_up_reason
                        });
                    }
                    marketLadderListsFourth.MarketLadderInfos.AddRange(MarketLadderInfoListFourth);
                    marketLadderList.MarketLadderLists.Add(marketLadderListsFourth);
                }

                if (root.data.third.limit_up_num > 0)
                {
                    var marketLadderListsThird = new MarketLadderList()
                    {
                        MarketLadderInfos = new List<MarketLadderInfo>()
                    };
                    marketLadderListsThird.MarketLadderBoard = "三连板";
                    marketLadderListsThird.MarketLadderNumber = root.data.third.limit_up_num + "只";
                    marketLadderListsThird.MarketLadderDescibe = string.Format("(晋级率：{0}%   炸板率：{1}%   竞价涨幅：{2}% )"
                        , root.data.third.promotion_rate, root.data.third.plate_frying_rate, root.data.third.call_auction_rise);

                    var MarketLadderInfoListThird = new List<MarketLadderInfo>();
                    for (int i = 0; i < root.data.third.limit_up_list.Count; i++)
                    {
                        MarketLadderInfoListThird.Add(new Dtos.MarketLadderInfo
                        {
                            MarketLadderCode = root.data.third.limit_up_list[i].stockCode,
                            MarketLadderName = root.data.third.limit_up_list[i].stockName,
                            MarketLadderFirstLimitUp = root.data.third.limit_up_list[i].first_limit_up_time,
                            MarketLadderReasonLimitUp = root.data.third.limit_up_list[i].limit_up_reason
                        });
                    }
                    marketLadderListsThird.MarketLadderInfos.AddRange(MarketLadderInfoListThird);
                    marketLadderList.MarketLadderLists.Add(marketLadderListsThird);
                }

                if (root.data.second.limit_up_num > 0)
                {
                    var marketLadderListsSecond = new MarketLadderList()
                    {
                        MarketLadderInfos = new List<MarketLadderInfo>()
                    };
                    marketLadderListsSecond.MarketLadderBoard = "二连板";
                    marketLadderListsSecond.MarketLadderNumber = root.data.second.limit_up_num + "只";
                    marketLadderListsSecond.MarketLadderDescibe = string.Format("(晋级率：{0}%   炸板率：{1}%   竞价涨幅：{2}% )"
                        , root.data.second.promotion_rate, root.data.second.plate_frying_rate, root.data.second.call_auction_rise);

                    var MarketLadderInfoListSecond = new List<MarketLadderInfo>();
                    for (int i = 0; i < root.data.second.limit_up_list.Count; i++)
                    {
                        MarketLadderInfoListSecond.Add(new Dtos.MarketLadderInfo
                        {
                            MarketLadderCode = root.data.second.limit_up_list[i].stockCode,
                            MarketLadderName = root.data.second.limit_up_list[i].stockName,
                            MarketLadderFirstLimitUp = root.data.second.limit_up_list[i].first_limit_up_time,
                            MarketLadderReasonLimitUp = root.data.second.limit_up_list[i].limit_up_reason
                        });
                    }
                    marketLadderListsSecond.MarketLadderInfos.AddRange(MarketLadderInfoListSecond);
                    marketLadderList.MarketLadderLists.Add(marketLadderListsSecond);
                }

                if (root.data.first.limit_up_num > 0)
                {
                    var marketLadderListsFirst = new MarketLadderList()
                    {
                        MarketLadderInfos = new List<MarketLadderInfo>()
                    };
                    marketLadderListsFirst.MarketLadderBoard = "首板";
                    marketLadderListsFirst.MarketLadderNumber = root.data.first.limit_up_num + "只";
                    marketLadderListsFirst.MarketLadderDescibe = string.Format("(晋级率：{0}%   炸板率：{1}%   竞价涨幅：{2}% )"
                        , root.data.first.promotion_rate, root.data.first.plate_frying_rate, root.data.first.call_auction_rise);

                    var MarketLadderInfoListFirst = new List<MarketLadderInfo>();
                    for (int i = 0; i < root.data.first.limit_up_list.Count; i++)
                    {
                        MarketLadderInfoListFirst.Add(new Dtos.MarketLadderInfo
                        {
                            MarketLadderCode = root.data.first.limit_up_list[i].stockCode,
                            MarketLadderName = root.data.first.limit_up_list[i].stockName,
                            MarketLadderFirstLimitUp = root.data.first.limit_up_list[i].first_limit_up_time,
                            MarketLadderReasonLimitUp = root.data.first.limit_up_list[i].limit_up_reason
                        });
                    }
                    marketLadderListsFirst.MarketLadderInfos.AddRange(MarketLadderInfoListFirst);
                    marketLadderList.MarketLadderLists.Add(marketLadderListsFirst);
                }
            }

            var urlNews = SystemConstantTwo.MarketLadderDataUrl + GetTimeStamp(date);
            var responseNews = _stockHttpClient.GetAsync(urlNews).Result;
            var contentNews = responseNews.Content.ReadAsStringAsync().Result;
            if (!string.IsNullOrEmpty(contentNews))
            {
                NewsBasicDataDto.Root root = JsonConvert.DeserializeObject<NewsBasicDataDto.Root>(contentNews);

                for (int i = 0; i < root.data.items.Count; i++)
                {
                    marketLadderList.MarketLadderNewsLists.Add(new MarketLadderNewsList
                    {
                        MarketNewsTitle = root.data.items[i].name,
                        MarketNewsType = root.data.items[i].description
                    });
                }
            }
            return marketLadderList;
        }

        private int GetTimeStamp(DateTime dt)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 8, 0, 0);
            return Convert.ToInt32((dt - dateTime).TotalSeconds);
        }
    }
}

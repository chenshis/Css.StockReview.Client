using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using StockReview.Api.Dtos;
using StockReview.Api.IApiService;
using StockReview.Infrastructure.Config;
using StockReview.Infrastructure.Config.HttpClients.HeepHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

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

        public PlateRotationDto GetPlateRotation(DateTime date)
        {

            var deviceId = "578e15c3-154e-f830-7a17-00c2d6b03140";
            var plateRotationDtoResult = new PlateRotationDto()
            {
                PlateRotationHeaderTitle = new PlateRotationHeaderTitle(),
                PlateRotationInfosOne = new List<PlateRotationInfo>(),
                PlateRotationInfosTwo = new List<PlateRotationInfo>(),
                PlateRotationInfosThree = new List<PlateRotationInfo>(),
                PlateRotationInfosFour = new List<PlateRotationInfo>(),
                PlateRotationInfosFive = new List<PlateRotationInfo>(),
                PlateRotationInfosSix = new List<PlateRotationInfo>(),
                PlateSharesLimitUpInfosOne = new List<PlateSharesLimitUpInfo>(),
                PlateSharesLimitUpInfosTwo = new List<PlateSharesLimitUpInfo>(),
                PlateSharesLimitUpInfosThree = new List<PlateSharesLimitUpInfo>(),
                PlateSharesLimitUpInfosFour = new List<PlateSharesLimitUpInfo>(),
                PlateSharesLimitUpInfosFive = new List<PlateSharesLimitUpInfo>(),
                PlateSharesLimitUpInfosSix = new List<PlateSharesLimitUpInfo>()
            };
            uage();

            int workDayToAdd = 6;
            int workDaysCount = 1;

            while (workDaysCount <= workDayToAdd)
            {
                var postDta = DateTime.Compare(date, DateTime.Now.Date) != 0 ?
                    "Order=1&a=RealRankingInfo&st=30&apiv=w28&Type=1&c=ZhiShuRanking&PhoneOSNew=1&Index=0&ZSType=7&Date=" + date.ToString("yyyy-MM-dd") :
                    "Order=1&a=RealRankingInfo&st=30&apiv=w28&Type=1&c=ZhiShuRanking&PhoneOSNew=1&Index=0&ZSType=7&";
                HttpHelper httpHelper = new HttpHelper();
                HttpItem item = new HttpItem
                {
                    URL = DateTime.Compare(date, DateTime.Now.Date) != 0 ? SystemConstantTwo.PlateRotationPostDataUrl : SystemConstantTwo.PlateRotationPostDayDataUrl,
                    Method = "POST",
                    Timeout = 100000,
                    ReadWriteTimeout = 30000,
                    IsToLower = false,
                    Cookie = "",
                    UserAgent = SusAgent,
                    Accept = "text/html, application/xhtml+xml, */*",
                    ContentType = "application/x-www-form-urlencoded",
                    Referer = "",
                    Postdata = postDta,
                    ResultType = ResultType.String,
                    ProtocolVersion = HttpVersion.Version11
                };
                string context = httpHelper.GetHtml(item).Html;

                if (context.Length > 460)
                {
                    PlateBasicDataDto.Root root = JsonConvert.DeserializeObject<PlateBasicDataDto.Root>(context);

                    switch (workDaysCount)
                    {
                        case 1:
                            for (int j = 0; j < root.list.Count; j++)
                            {
                                plateRotationDtoResult.PlateRotationInfosOne.Add(new PlateRotationInfo
                                {
                                    Number = j.ToString(),
                                    PlateCode = root.list[j][0],
                                    PlateName = root.list[j][1],
                                    PlateStrength = root.list[j][2],
                                    PlateMainForce = root.list[j][3],
                                    PlateMainBuy = root.list[j][4],
                                    PlateMainSell = root.list[j][5]
                                });
                            }

                            string postDtaOne = DateTime.Compare(date, DateTime.Now.Date) != 0 ? "Order=1&a=ZhiShuStockList_W8&st=30&c=ZhiShuRanking&PhoneOSNew=1&old=1&DeviceID=" + deviceId + "&VerSion=5.13.0.0&old=1&IsZZ=0&Index=0&Date=" + date + "&apiv=w35&Type=6&PlateID=" + root.list[0][0] + "&" :
    "Order=1&a=ZhiShuStockList_W8&st=30&c=ZhiShuRanking&PhoneOSNew=1&DeviceID=" + deviceId + "&VerSion=5.13.0.0&old=1&IsZZ=0&Token=&Index=0&apiv=w35&Type=6&IsKZZType=0&PlateID=" + root.list[0][0] + "&";
                            HttpHelper httpHelperOne = new HttpHelper();
                            HttpItem itemOne = new HttpItem
                            {
                                URL = DateTime.Compare(date, DateTime.Now.Date) != 0 ? SystemConstantTwo.PlateRotationPostDataUrl : SystemConstantTwo.PlateRotationPostDayDataUrl,
                                Method = "POST",
                                Timeout = 100000,
                                ReadWriteTimeout = 30000,
                                IsToLower = false,
                                Cookie = "",
                                UserAgent = SusAgent,
                                Accept = "text/html, application/xhtml+xml, */*",
                                ContentType = "application/x-www-form-urlencoded",
                                Referer = "",
                                Postdata = postDtaOne,
                                ResultType = ResultType.String,
                                ProtocolVersion = HttpVersion.Version11
                            };
                            string contextOne = httpHelperOne.GetHtml(itemOne).Html;

                            if (!string.IsNullOrWhiteSpace(contextOne))
                            {
                                PlateBasicDataDto.Root rootOne= JsonConvert.DeserializeObject<PlateBasicDataDto.Root>(contextOne);

                                for (int k = 0; k < rootOne.list.Count; k++)
                                {
                                    plateRotationDtoResult.PlateSharesLimitUpInfosOne.Add(new PlateSharesLimitUpInfo
                                    {
                                        Number = k.ToString(),
                                        PlateSharesCode = rootOne.list[k][0],
                                        PlateSharesName = rootOne.list[k][1],
                                        PlateSharesPrice = rootOne.list[k][5],
                                        PlateSharesIncrease = rootOne.list[k][6],
                                        PlateSharesStatus = rootOne.list[k][24],
                                        PlateSharesNumberBoards = rootOne.list[k][23],
                                        PlateSharesMainForce = rootOne.list[k][2],
                                        PlateSharesToName = rootOne.list[k][4],
                                        PlateSharesTranVoume = rootOne.list[k][7],
                                        PlateSharesCirculationValue = rootOne.list[k][38],
                                        PlateSharesTest = rootOne.list[k][24]
                                    });
                                }
                            }
                            plateRotationDtoResult.PlateRotationHeaderTitle.PlateDateOne = date.ToString("yyyy-MM-dd");
                            workDaysCount++;
                            break;
                        case 2:
                            for (int j = 0; j < root.list.Count; j++)
                            {
                                plateRotationDtoResult.PlateRotationInfosTwo.Add(new PlateRotationInfo
                                {
                                    Number = j + 1.ToString(),
                                    PlateCode = root.list[j][0],
                                    PlateName = root.list[j][1],
                                    PlateStrength = root.list[j][2],
                                    PlateMainForce = root.list[j][3],
                                    PlateMainBuy = root.list[j][4],
                                    PlateMainSell = root.list[j][5]
                                });
                            }

                            string postDtaTwo = DateTime.Compare(date, DateTime.Now.Date) != 0 ? "Order=1&a=ZhiShuStockList_W8&st=30&c=ZhiShuRanking&PhoneOSNew=1&old=1&DeviceID=" + deviceId + "&VerSion=5.13.0.0&old=1&IsZZ=0&Index=0&Date=" + date + "&apiv=w35&Type=6&PlateID=" + root.list[0][0] + "&" :
"Order=1&a=ZhiShuStockList_W8&st=30&c=ZhiShuRanking&PhoneOSNew=1&DeviceID=" + deviceId + "&VerSion=5.13.0.0&old=1&IsZZ=0&Token=&Index=0&apiv=w35&Type=6&IsKZZType=0&PlateID=" + root.list[0][0] + "&";
                            HttpHelper httpHelperTwo = new HttpHelper();
                            HttpItem itemTwo = new HttpItem
                            {
                                URL = DateTime.Compare(date, DateTime.Now.Date) != 0 ? SystemConstantTwo.PlateRotationPostDataUrl : SystemConstantTwo.PlateRotationPostDayDataUrl,
                                Method = "POST",
                                Timeout = 100000,
                                ReadWriteTimeout = 30000,
                                IsToLower = false,
                                Cookie = "",
                                UserAgent = SusAgent,
                                Accept = "text/html, application/xhtml+xml, */*",
                                ContentType = "application/x-www-form-urlencoded",
                                Referer = "",
                                Postdata = postDtaTwo,
                                ResultType = ResultType.String,
                                ProtocolVersion = HttpVersion.Version11
                            };
                            string contextTwo = httpHelperTwo.GetHtml(itemTwo).Html;

                            if (!string.IsNullOrWhiteSpace(contextTwo))
                            {
                                PlateBasicDataDto.Root rootTwo = JsonConvert.DeserializeObject<PlateBasicDataDto.Root>(contextTwo);

                                for (int k = 0; k < rootTwo.list.Count; k++)
                                {
                                    plateRotationDtoResult.PlateSharesLimitUpInfosTwo.Add(new PlateSharesLimitUpInfo
                                    {
                                        Number = k.ToString(),
                                        PlateSharesCode = rootTwo.list[k][0],
                                        PlateSharesName = rootTwo.list[k][1],
                                        PlateSharesPrice = rootTwo.list[k][5],
                                        PlateSharesIncrease = rootTwo.list[k][6],
                                        PlateSharesStatus = rootTwo.list[k][24],
                                        PlateSharesNumberBoards = rootTwo.list[k][23],
                                        PlateSharesMainForce = rootTwo.list[k][2],
                                        PlateSharesToName = rootTwo.list[k][4],
                                        PlateSharesTranVoume = rootTwo.list[k][7],
                                        PlateSharesCirculationValue = rootTwo.list[k][38],
                                        PlateSharesTest = rootTwo.list[k][24]
                                    });
                                }
                            }
                            plateRotationDtoResult.PlateRotationHeaderTitle.PlateDateTwo = date.ToString("yyyy-MM-dd");
                            workDaysCount++;
                            break;
                        case 3:
                            for (int j = 0; j < root.list.Count; j++)
                            {
                                plateRotationDtoResult.PlateRotationInfosThree.Add(new PlateRotationInfo
                                {
                                    Number = j + 1.ToString(),
                                    PlateCode = root.list[j][0],
                                    PlateName = root.list[j][1],
                                    PlateStrength = root.list[j][2],
                                    PlateMainForce = root.list[j][3],
                                    PlateMainBuy = root.list[j][4],
                                    PlateMainSell = root.list[j][5]
                                });
                            }

                            string postDtaThree = DateTime.Compare(date, DateTime.Now.Date) != 0 ? "Order=1&a=ZhiShuStockList_W8&st=30&c=ZhiShuRanking&PhoneOSNew=1&old=1&DeviceID=" + deviceId + "&VerSion=5.13.0.0&old=1&IsZZ=0&Index=0&Date=" + date + "&apiv=w35&Type=6&PlateID=" + root.list[0][0] + "&" :
                                "Order=1&a=ZhiShuStockList_W8&st=30&c=ZhiShuRanking&PhoneOSNew=1&DeviceID=" + deviceId + "&VerSion=5.13.0.0&old=1&IsZZ=0&Token=&Index=0&apiv=w35&Type=6&IsKZZType=0&PlateID=" + root.list[0][0] + "&";
                            HttpHelper httpHelperThree = new HttpHelper();
                            HttpItem itemThree = new HttpItem
                            {
                                URL = DateTime.Compare(date, DateTime.Now.Date) != 0 ? SystemConstantTwo.PlateRotationPostDataUrl : SystemConstantTwo.PlateRotationPostDayDataUrl,
                                Method = "POST",
                                Timeout = 100000,
                                ReadWriteTimeout = 30000,
                                IsToLower = false,
                                Cookie = "",
                                UserAgent = SusAgent,
                                Accept = "text/html, application/xhtml+xml, */*",
                                ContentType = "application/x-www-form-urlencoded",
                                Referer = "",
                                Postdata = postDtaThree,
                                ResultType = ResultType.String,
                                ProtocolVersion = HttpVersion.Version11
                            };
                            string contextThree = httpHelperThree.GetHtml(itemThree).Html;

                            if (!string.IsNullOrWhiteSpace(contextThree))
                            {
                                PlateBasicDataDto.Root rootThree = JsonConvert.DeserializeObject<PlateBasicDataDto.Root>(contextThree);

                                for (int k = 0; k < rootThree.list.Count; k++)
                                {
                                    plateRotationDtoResult.PlateSharesLimitUpInfosThree.Add(new PlateSharesLimitUpInfo
                                    {
                                        Number = k.ToString(),
                                        PlateSharesCode = rootThree.list[k][0],
                                        PlateSharesName = rootThree.list[k][1],
                                        PlateSharesPrice = rootThree.list[k][5],
                                        PlateSharesIncrease = rootThree.list[k][6],
                                        PlateSharesStatus = rootThree.list[k][24],
                                        PlateSharesNumberBoards = rootThree.list[k][23],
                                        PlateSharesMainForce = rootThree.list[k][2],
                                        PlateSharesToName = rootThree.list[k][4],
                                        PlateSharesTranVoume = rootThree.list[k][7],
                                        PlateSharesCirculationValue = rootThree.list[k][38],
                                        PlateSharesTest = rootThree.list[k][24]
                                    });
                                }
                            }
                            plateRotationDtoResult.PlateRotationHeaderTitle.PlateDateThree = date.ToString("yyyy-MM-dd");
                            workDaysCount++;
                            break;
                        case 4:
                            for (int j = 0; j < root.list.Count; j++)
                            {
                                plateRotationDtoResult.PlateRotationInfosFour.Add(new PlateRotationInfo
                                {
                                    Number = j + 1.ToString(),
                                    PlateCode = root.list[j][0],
                                    PlateName = root.list[j][1],
                                    PlateStrength = root.list[j][2],
                                    PlateMainForce = root.list[j][3],
                                    PlateMainBuy = root.list[j][4],
                                    PlateMainSell = root.list[j][5]
                                });
                            }

                            string postDtaFour = DateTime.Compare(date, DateTime.Now.Date) != 0 ? "Order=1&a=ZhiShuStockList_W8&st=30&c=ZhiShuRanking&PhoneOSNew=1&old=1&DeviceID=" + deviceId + "&VerSion=5.13.0.0&old=1&IsZZ=0&Index=0&Date=" + date + "&apiv=w35&Type=6&PlateID=" + root.list[0][0] + "&" :
"Order=1&a=ZhiShuStockList_W8&st=30&c=ZhiShuRanking&PhoneOSNew=1&DeviceID=" + deviceId + "&VerSion=5.13.0.0&old=1&IsZZ=0&Token=&Index=0&apiv=w35&Type=6&IsKZZType=0&PlateID=" + root.list[0][0] + "&";
                            HttpHelper httpHelperFour = new HttpHelper();
                            HttpItem itemFour = new HttpItem
                            {
                                URL = DateTime.Compare(date, DateTime.Now.Date) != 0 ? SystemConstantTwo.PlateRotationPostDataUrl : SystemConstantTwo.PlateRotationPostDayDataUrl,
                                Method = "POST",
                                Timeout = 100000,
                                ReadWriteTimeout = 30000,
                                IsToLower = false,
                                Cookie = "",
                                UserAgent = SusAgent,
                                Accept = "text/html, application/xhtml+xml, */*",
                                ContentType = "application/x-www-form-urlencoded",
                                Referer = "",
                                Postdata = postDtaFour,
                                ResultType = ResultType.String,
                                ProtocolVersion = HttpVersion.Version11
                            };
                            string contextFour = httpHelperFour.GetHtml(itemFour).Html;

                            if (!string.IsNullOrWhiteSpace(contextFour))
                            {
                                PlateBasicDataDto.Root rootFour = JsonConvert.DeserializeObject<PlateBasicDataDto.Root>(contextFour);

                                for (int k = 0; k < rootFour.list.Count; k++)
                                {
                                    plateRotationDtoResult.PlateSharesLimitUpInfosFour.Add(new PlateSharesLimitUpInfo
                                    {
                                        Number = k.ToString(),
                                        PlateSharesCode = rootFour.list[k][0],
                                        PlateSharesName = rootFour.list[k][1],
                                        PlateSharesPrice = rootFour.list[k][5],
                                        PlateSharesIncrease = rootFour.list[k][6],
                                        PlateSharesStatus = rootFour.list[k][24],
                                        PlateSharesNumberBoards = rootFour.list[k][23],
                                        PlateSharesMainForce = rootFour.list[k][2],
                                        PlateSharesToName = rootFour.list[k][4],
                                        PlateSharesTranVoume = rootFour.list[k][7],
                                        PlateSharesCirculationValue = rootFour.list[k][38],
                                        PlateSharesTest = rootFour.list[k][24]
                                    });
                                }
                            }

                            plateRotationDtoResult.PlateRotationHeaderTitle.PlateDateFour = date.ToString("yyyy-MM-dd");
                            workDaysCount++;
                            break;
                        case 5:
                            for (int j = 0; j < root.list.Count; j++)
                            {
                                plateRotationDtoResult.PlateRotationInfosFive.Add(new PlateRotationInfo
                                {
                                    Number = j + 1.ToString(),
                                    PlateCode = root.list[j][0],
                                    PlateName = root.list[j][1],
                                    PlateStrength = root.list[j][2],
                                    PlateMainForce = root.list[j][3],
                                    PlateMainBuy = root.list[j][4],
                                    PlateMainSell = root.list[j][5]
                                });
                            }

                                string postDtaFive = DateTime.Compare(date, DateTime.Now.Date) != 0 ? "Order=1&a=ZhiShuStockList_W8&st=30&c=ZhiShuRanking&PhoneOSNew=1&old=1&DeviceID=" + deviceId + "&VerSion=5.13.0.0&old=1&IsZZ=0&Index=0&Date=" + date + "&apiv=w35&Type=6&PlateID=" + root.list[0][0] + "&" :
   "Order=1&a=ZhiShuStockList_W8&st=30&c=ZhiShuRanking&PhoneOSNew=1&DeviceID=" + deviceId + "&VerSion=5.13.0.0&old=1&IsZZ=0&Token=&Index=0&apiv=w35&Type=6&IsKZZType=0&PlateID=" + root.list[0][0] + "&";
                                HttpHelper httpHelperFive = new HttpHelper();
                                HttpItem itemFive = new HttpItem
                                {
                                    URL = DateTime.Compare(date, DateTime.Now.Date) != 0 ? SystemConstantTwo.PlateRotationPostDataUrl : SystemConstantTwo.PlateRotationPostDayDataUrl,
                                    Method = "POST",
                                    Timeout = 100000,
                                    ReadWriteTimeout = 30000,
                                    IsToLower = false,
                                    Cookie = "",
                                    UserAgent = SusAgent,
                                    Accept = "text/html, application/xhtml+xml, */*",
                                    ContentType = "application/x-www-form-urlencoded",
                                    Referer = "",
                                    Postdata = postDtaFive,
                                    ResultType = ResultType.String,
                                    ProtocolVersion = HttpVersion.Version11
                                };
                                string contextFive = httpHelperFive.GetHtml(itemFive).Html;

                            if (!string.IsNullOrWhiteSpace(contextFive))
                            {
                                PlateBasicDataDto.Root rootFive = JsonConvert.DeserializeObject<PlateBasicDataDto.Root>(contextFive);

                                for (int k = 0; k < rootFive.list.Count; k++)
                                {
                                    plateRotationDtoResult.PlateSharesLimitUpInfosFive.Add(new PlateSharesLimitUpInfo
                                    {
                                        Number = k.ToString(),
                                        PlateSharesCode = rootFive.list[k][0],
                                        PlateSharesName = rootFive.list[k][1],
                                        PlateSharesPrice = rootFive.list[k][5],
                                        PlateSharesIncrease = rootFive.list[k][6],
                                        PlateSharesStatus = rootFive.list[k][24],
                                        PlateSharesNumberBoards = rootFive.list[k][23],
                                        PlateSharesMainForce = rootFive.list[k][2],
                                        PlateSharesToName = rootFive.list[k][4],
                                        PlateSharesTranVoume = rootFive.list[k][7],
                                        PlateSharesCirculationValue = rootFive.list[k][38],
                                        PlateSharesTest = rootFive.list[k][24]
                                    });
                                }
                            }
                            plateRotationDtoResult.PlateRotationHeaderTitle.PlateDateFive = date.ToString("yyyy-MM-dd");
                            workDaysCount++;
                            break;
                        case 6:
                            for (int j = 0; j < root.list.Count; j++)
                            {
                                plateRotationDtoResult.PlateRotationInfosSix.Add(new PlateRotationInfo
                                {
                                    Number = j + 1.ToString(),
                                    PlateCode = root.list[j][0],
                                    PlateName = root.list[j][1],
                                    PlateStrength = root.list[j][2],
                                    PlateMainForce = root.list[j][3],
                                    PlateMainBuy = root.list[j][4],
                                    PlateMainSell = root.list[j][5]
                                });
                            }

                                string postDtaSix = DateTime.Compare(date, DateTime.Now.Date) != 0 ? "Order=1&a=ZhiShuStockList_W8&st=30&c=ZhiShuRanking&PhoneOSNew=1&old=1&DeviceID=" + deviceId + "&VerSion=5.13.0.0&old=1&IsZZ=0&Index=0&Date=" + date + "&apiv=w35&Type=6&PlateID=" + root.list[0][0] + "&" :
   "Order=1&a=ZhiShuStockList_W8&st=30&c=ZhiShuRanking&PhoneOSNew=1&DeviceID=" + deviceId + "&VerSion=5.13.0.0&old=1&IsZZ=0&Token=&Index=0&apiv=w35&Type=6&IsKZZType=0&PlateID=" + root.list[0][0] + "&";
                                HttpHelper httpHelperSix = new HttpHelper();
                                HttpItem itemSix = new HttpItem
                                {
                                    URL = DateTime.Compare(date, DateTime.Now.Date) != 0 ? SystemConstantTwo.PlateRotationPostDataUrl : SystemConstantTwo.PlateRotationPostDayDataUrl,
                                    Method = "POST",
                                    Timeout = 100000,
                                    ReadWriteTimeout = 30000,
                                    IsToLower = false,
                                    Cookie = "",
                                    UserAgent = SusAgent,
                                    Accept = "text/html, application/xhtml+xml, */*",
                                    ContentType = "application/x-www-form-urlencoded",
                                    Referer = "",
                                    Postdata = postDtaSix,
                                    ResultType = ResultType.String,
                                    ProtocolVersion = HttpVersion.Version11
                                };
                                string contextSix = httpHelperSix.GetHtml(itemSix).Html;

                            if (!string.IsNullOrWhiteSpace(contextSix))
                            {
                                PlateBasicDataDto.Root rootSix = JsonConvert.DeserializeObject<PlateBasicDataDto.Root>(contextSix);

                                for (int k = 0; k < rootSix.list.Count; k++)
                                {
                                    plateRotationDtoResult.PlateSharesLimitUpInfosSix.Add(new PlateSharesLimitUpInfo
                                    {
                                        Number = k.ToString(),
                                        PlateSharesCode = rootSix.list[k][0],
                                        PlateSharesName = rootSix.list[k][1],
                                        PlateSharesPrice = rootSix.list[k][5],
                                        PlateSharesIncrease = rootSix.list[k][6],
                                        PlateSharesStatus = rootSix.list[k][24],
                                        PlateSharesNumberBoards = rootSix.list[k][23],
                                        PlateSharesMainForce = rootSix.list[k][2],
                                        PlateSharesToName = rootSix.list[k][4],
                                        PlateSharesTranVoume = rootSix.list[k][7],
                                        PlateSharesCirculationValue = rootSix.list[k][38],
                                        PlateSharesTest = rootSix.list[k][24]
                                    });
                                }
                            }
                            plateRotationDtoResult.PlateRotationHeaderTitle.PlateDateSix = date.ToString("yyyy-MM-dd");
                            workDaysCount++;

                            break;
                        default:
                            break;
                    }
                }
                date = date.AddDays(-1);
            }
            return plateRotationDtoResult;
        }

        public ExplosiveBoardLImitDownDto GetExplosiveBoardLImitDown(DateTime date)
        {
            var result = new ExplosiveBoardLImitDownDto()
            {
                ExplosiveFriedIndividualInfos = new List<ExplosiveFriedIndividualInfo>(),
                ExplosiveLimitDownStaticsInfos = new List<ExplosiveLimitDownStaticsInfo>(),
                ExplosiveLimitUpStaticsInfos = new List<ExplosiveLimitUpStaticsInfo>(),
                ExplosiveYeasterdayLimitUpStaticsInfos = new List<ExplosiveYeasterdayLimitUpStaticsInfo>()
            };
            var url = SystemConstantTwo.ExplosivePostDataUrl + "?pool_name=limit_up_broken" + date.ToString("yyyyMMdd");
            var response = _stockHttpClient.GetAsync(url).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            if (!string.IsNullOrEmpty(content))
            {
                if (content != null && !(content.Substring(0, 1) != "{") && content.Length >= 120)
                {
                    ZaBanBasicDataDto.Root root = JsonConvert.DeserializeObject<ZaBanBasicDataDto.Root>(content);

                    for (int i = 0; i < root.data.Count; i++)
                    {
                        var explosiveFriedIndividualInfo = new ExplosiveFriedIndividualInfo();
                        explosiveFriedIndividualInfo.Number = i + 1;
                        explosiveFriedIndividualInfo.ExpSharesCode = root.data[i].symbol.ToString().Substring(0, 6);
                        explosiveFriedIndividualInfo.ExpSharesName = root.data[i].stock_chi_name;
                        explosiveFriedIndividualInfo.ExpSharesStartConnectedBoard = root.data[i].yesterday_limit_up_days;
                        explosiveFriedIndividualInfo.ExpSharesFirstSealingTime = GetDateTime(root.data[i].first_limit_up.ToString());
                        explosiveFriedIndividualInfo.ExpSharesTailSealingTime = GetDateTime(root.data[i].last_limit_up.ToString());
                        explosiveFriedIndividualInfo.ExpSharesStartDoingBusiness = root.data[i].break_limit_up_times;
                        explosiveFriedIndividualInfo.ExpSharesCirculatingMarketValue = zEy(root.data[i].non_restricted_capital);
                        explosiveFriedIndividualInfo.ExpSharesLastFryingTime = GetDateTime(root.data[i].last_break_limit_up.ToString());
                        explosiveFriedIndividualInfo.ExpSharesConcept = root.data[i].surge_reason.related_plates[0].plate_name;
                        result.ExplosiveFriedIndividualInfos.Add(explosiveFriedIndividualInfo);
                    }

                }
            }
            var urlLimitUp = SystemConstantTwo.ExplosivePostDataUrl + "?pool_name=yesterday_limit_up" + date.AddDays(-1).ToString("yyyyMMdd");
            var responseLimitUp = _stockHttpClient.GetAsync(urlLimitUp).Result;
            var contentLimitUp = responseLimitUp.Content.ReadAsStringAsync().Result;
            if (!string.IsNullOrEmpty(contentLimitUp))
            {
                ExplosiveBasicDataDto.Root root = JsonConvert.DeserializeObject<ExplosiveBasicDataDto.Root>(contentLimitUp);
                for (int i = 0; i < root.data.Count; i++)
                {
                    var dataInfo = new ExplosiveYeasterdayLimitUpStaticsInfo();
                    dataInfo.Number = i + 1;
                    dataInfo.ExpYeaCode = root.data[i].symbol.ToString().Substring(0, 6);
                    dataInfo.ExpYeaName = root.data[i].stock_chi_name;
                    dataInfo.ExpYeaIncrease = GetPercentage(root.data[i].change_percent);
                    dataInfo.ExpYeaConsecutiveBoard = root.data[i].yesterday_limit_up_days;
                    if (root.data[i].surge_reason != null)
                    {
                        dataInfo.ExpYeaModule = root.data[i].surge_reason.related_plates[0].plate_name;
                    }
                    dataInfo.ExpYeaChange = "1";
                    result.ExplosiveYeasterdayLimitUpStaticsInfos.Add(dataInfo);
                }

                if (result.ExplosiveYeasterdayLimitUpStaticsInfos.Count > 0)
                {
                    var numberCount = 1;
                    result.ExplosiveLimitUpStaticsInfos = result.ExplosiveYeasterdayLimitUpStaticsInfos.GroupBy(x => x.ExpYeaModule).Select(x => new ExplosiveLimitUpStaticsInfo
                    {
                        Number = numberCount++,
                        ExpLimitName = x.Key,
                        ExpLimitTotal = x.Count(),
                        ExpLimitUp = x.Where(x => x.ExpYeaIncrease > 0).Count(),
                        ExpLimitDown = x.Where(x => x.ExpYeaIncrease < 0).Count(),
                    }).ToList();
                }

            }


            var urlLimitDown = SystemConstantTwo.ExplosivePostDataUrl + "?pool_name=limit_down" + date.ToString("yyyyMMdd");
            var responseLimitDown = _stockHttpClient.GetAsync(urlLimitDown).Result;
            var contentLimitDown = responseLimitDown.Content.ReadAsStringAsync().Result;
            if (!string.IsNullOrEmpty(contentLimitDown))
            {
                ExplosiveBasicDataDto.Root root = JsonConvert.DeserializeObject<ExplosiveBasicDataDto.Root>(contentLimitDown);
                for (int i = 0; i < root.data.Count; i++)
                {
                    var dataInfo = new ExplosiveLimitDownStaticsInfo();
                    dataInfo.Number = i + 1;
                    dataInfo.ExpDownCode = root.data[i].symbol.ToString().Substring(0, 6);
                    dataInfo.ExpDownName = root.data[i].stock_chi_name;
                    dataInfo.ExpDownFirstSealingTime= GetDateTime(root.data[i].first_limit_up.ToString());
                    dataInfo.ExpDownTailSealingTime= GetDateTime(root.data[i].last_limit_up.ToString());
                    dataInfo.ExpDownStartConnectedBoard = root.data[i].break_limit_up_times;
                    dataInfo.ExpDownStartDoingBusiness = root.data[i].limit_down_days;

                    result.ExplosiveLimitDownStaticsInfos.Add(dataInfo);

                }

            }
            return result;
        }

        public DragonTigerDto GetDragonTiger(DateTime date)
        {
            var result = new DragonTigerDto()
            {
                DragonTigerGetInfos = new List<DragonTigerGetInfo>(),
                DragonTigerGetInfosOne = new List<DragonTigerGetInfo>(),
                DragonTigerGetInfosTwo = new List<DragonTigerGetInfo>(),
                DragonTigerGetInfosFous = new List<DragonTigerGetInfo>(),
                DragonTigerGetInfosThree = new List<DragonTigerGetInfo>()
            };

            int workDayToAdd = 5;
            int workDaysCount = 1;

            while (workDaysCount <= workDayToAdd)
            {
                var postDta = "{\"Params\":[\"jm\"," + date.ToString("yyyy-MM-dd") + ",\"jmr\",1]}";
                HttpHelper httpHelper = new HttpHelper();
                HttpItem item = new HttpItem
                {
                    URL = "http://page.tdx.com.cn:7615/TQLEX?Entry=CWServ.cfg_fx_yzlhb_lhb",
                    Method = "POST",
                    Timeout = 100000,
                    ReadWriteTimeout = 30000,
                    IsToLower = false,
                    Cookie = "",
                    UserAgent = "Mozilla/5.0 (Linux; Android 7.1.2; SM-G973N Build/PPR1.190810.011; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/92.0.4515.131 Mobile Safari/537.36;kaipanla 5.0.0.2",
                    Accept = "application/json, text/javascript, */*; q=0.01",
                    ContentType = "application/x-www-form-urlencoded",
                    Referer = "http://page.tdx.com.cn:7615/site/kggx/tk_yzlhb_yz.html",
                    Postdata = postDta,
                    ResultType = ResultType.String,
                    ProtocolVersion = HttpVersion.Version11
                };
                string context = httpHelper.GetHtml(item).Html;

                if (!string.IsNullOrEmpty(context)&& context.Length>170)
                {
                    DragonTigerBasicDataDto.Root root = JsonConvert.DeserializeObject<DragonTigerBasicDataDto.Root>(context);
                    for (int i = 0; i < root.ResultSets[0].Content.Count; i++)
                    {
                        var dragonTigerInfos = new DragonTigerGetInfo();

                        dragonTigerInfos.DragonName = root.ResultSets[0].Content[i][1].ToString() + Environment.NewLine + root.ResultSets[0].Content[i][2].ToString();
                        string dragonSpeculative = root.ResultSets[0].Content[i][5];
                        if (root.ResultSets[0].Content[i][3].Contains("3r"))
                        {
                            dragonTigerInfos.DragonSpeculative = "3日榜," + dragonSpeculative;
                        }
                        else
                        {
                            dragonTigerInfos.DragonSpeculative = dragonSpeculative;
                        }

                        string dragonIncrease = root.ResultSets[0].Content[i][6];
                        if (dragonIncrease != null)
                        {
                            if (!(dragonIncrease.Substring(0, 1) != "-"))
                            {

                                dragonTigerInfos.DragonIncreaseColor = "Green";
                            }
                            else
                            {
                                dragonTigerInfos.DragonIncreaseColor = "Red";
                            }
                            dragonTigerInfos.DragonIncrease = dragonIncrease;
                        }

                        double num = Convert.ToDouble(root.ResultSets[0].Content[i][4]);
                        if (Math.Abs(num) < 100000000.0)
                        {
                           
                            if (num > 0.0)
                            {
                                dragonTigerInfos.DragonPurchaseColor = "Green";
                            }
                            else
                            {
                                dragonTigerInfos.DragonPurchaseColor = "Red";
                            }
                            dragonTigerInfos.DragonPurchase = Math.Round(num * 0.0001, 0) + "万";
                        }
                        else
                        {
                         
                            if (num <= 0.0)
                            {
                                dragonTigerInfos.DragonPurchaseColor = "Green";
                            }
                            else
                            {
                                dragonTigerInfos.DragonPurchaseColor = "Red";
                            }
                            dragonTigerInfos.DragonPurchase = Math.Round(num * 1E-08, 2) + "亿";
                        }
                        switch (workDaysCount)
                        {
                            case 1: result.DragonTigerGetInfos.Add(dragonTigerInfos); break;
                            case 2: result.DragonTigerGetInfosOne.Add(dragonTigerInfos); break;
                            case 3: result.DragonTigerGetInfosTwo.Add(dragonTigerInfos); break;
                            case 4: result.DragonTigerGetInfosFous.Add(dragonTigerInfos); break;
                            case 5: result.DragonTigerGetInfosThree.Add(dragonTigerInfos); break;
                            default:
                                break;
                        }
                       
                    }
                    if (workDaysCount == 1 && result.DragonTigerGetInfos.Count > 0)
                    {
                        var urlLimitUp = SystemConstantTwo.ExplosivePostDataUrl + "?pool_name=limit_up&date=" + date.ToString("yyyy-MM-dd");
                        var responseLimitUp = _stockHttpClient.GetAsync(urlLimitUp).Result;
                        var contentLimitUp = responseLimitUp.Content.ReadAsStringAsync().Result;
                        if (!string.IsNullOrEmpty(contentLimitUp))
                        {
                            JsClassBasicDataDto.Root rootOne = JsonConvert.DeserializeObject<JsClassBasicDataDto.Root>(contentLimitUp);
                            string text2 = "";
                            for (int i = 0; i < result.DragonTigerGetInfos.Count; i++)
                            {
                                if (!(double.Parse(result.DragonTigerGetInfos[i].DragonIncrease.ToString()) > 9.5))
                                {
                                    continue;
                                }
                                string text3 = result.DragonTigerGetInfos[i].DragonName.ToString();
                                for (int j = 0; j < rootOne.data.Count; j++)
                                {
                                    text2 = rootOne.data[j].symbol.Substring(0, 6);
                                    if (text3.Contains(text2))
                                    {
                                        int limit_up_days = rootOne.data[j].limit_up_days;
                                        result.DragonTigerGetInfos[i].DragonPlate = limit_up_days>0? limit_up_days.ToString():"";
                                    }
                                }
                            }
                        }
                    }

                    workDaysCount++;
                }
                date = date.AddDays(-1);
            }

            return result;
        }

        #region 私有方法
        public double GetPercentage(double strlongdou)
        {
            new NumberFormatInfo().PercentDecimalDigits = 2;
            return Math.Round(Convert.ToDouble(strlongdou) * 100.0, 2);
        }

        public string SusAgent;
        public double zEy(double e)
        {
            return Math.Round(e * 1E-08, 2);
        }

        public DateTime GetDateTime(string strLongTime)
        {
            long num = Convert.ToInt64(strLongTime) * 10000000L;
            long ticks = new DateTime(1970, 1, 1, 8, 0, 0).Ticks + num;
            return new DateTime(ticks);
        }
        private string GetRandUag(int length)
        {
            char[] array = new char[3] { '1', '2', '3' };
            string text = string.Empty;
            Random random = new Random(GetNewSeed());
            for (int i = 0; i < length; i++)
            {
                text += array[random.Next(array.Length)];
            }
            return text;
        }

        private static int GetNewSeed()
        {
            byte[] array = new byte[4];
            new RNGCryptoServiceProvider().GetBytes(array);
            return BitConverter.ToInt32(array, 0);
        }

        private void uage()
        {
            int userAgent = Convert.ToInt32(GetRandUag(1));
            SusAgent = SetUserAgent(userAgent);
        }

        private int GetTimeStamp(DateTime dt)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 8, 0, 0);
            return Convert.ToInt32((dt - dateTime).TotalSeconds);
        }

        private string SetUserAgent(int x)
        {
            string text = "";
            return x switch
            {
                1 => "Opera/9.27 (Linux; U; Android 8.1.0; zh-cn; BLA-AL00 Build/HUAWEIBLA-AL00) Chrome/57.0.2987.132 Mobile Safari/437.26",
                2 => "Opera/8.36 (Macintosh; Intel Mac OS X 10_15_4) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.113 Safari/337.3",
                3 => "Opera/7.63 (Macintosh; Intel Mac OS X 10_11_6) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.102 Safari/517.6",
                _ => "Opera/6.85 (Macintosh; Intel Mac OS X 10.11; rv:47.0) Gecko/20100101 Firefox/47.0",
            };
        }
        #endregion
    }
}

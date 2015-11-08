﻿using AllReady.Areas.Admin.Models;
using AllReady.Models;
using MediatR;
using Microsoft.Data.Entity;
using System.Linq;

namespace AllReady.Areas.Admin.Features.Campaigns
{
    public class CampaignDetailQueryHandler : IRequestHandler<CampaignDetailQuery, CampaignDetailModel>
    {
        private AllReadyContext _context;

        public CampaignDetailQueryHandler(AllReadyContext context)
        {
            _context = context;

        }
        public CampaignDetailModel Handle(CampaignDetailQuery message)
        {
            var campaign = _context.Campaigns
                                  .AsNoTracking()
                                  .Include(c => c.Activities)    
                                  .Include(m => m.ManagingTenant)
                                  .SingleOrDefault(c => c.Id == message.CampaignId);

            CampaignDetailModel result = null;

            if (campaign != null)
            {
                result = new CampaignDetailModel()
                {
                    Id = campaign.Id,
                    Name = campaign.Name,
                    Description = campaign.Description,
                    TenantId = campaign.ManagingTenantId,
                    TenantName = campaign.ManagingTenant.Name,
                    ImageUrl = campaign.ImageUrl,
                    StartDate = campaign.StartDateTimeUtc,
                    EndDate = campaign.EndDateTimeUtc,
                    Activities = campaign.Activities.Select(a => new ActivitySummaryModel
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Description = a.Description,
                        StartDateTime = a.StartDateTimeUtc,
                        EndDateTime = a.EndDateTimeUtc,
                        CampaignId = campaign.Id,
                        CampaignName = campaign.Name,
                        TenantId = campaign.ManagingTenantId,
                        TenantName = campaign.ManagingTenant.Name,
                        ImageUrl = a.ImageUrl
                    })
                };
            }
            return result;
        }
    }
}
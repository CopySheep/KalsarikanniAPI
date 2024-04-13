using HotelFuen31.APIs.Dtos.RenYu;
using HotelFuen31.APIs.Models;
using HotelFuen31.APIs.Uitilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace HotelFuen31.APIs.Services.RenYu
{
    public class NotificationService
    {
        private readonly AppDbContext _context;
        public NotificationService(AppDbContext context)
        {
            _context = context;
        }

        public AllNotificationPageDto GetAll(int page = 1, int pageSize = 10)
        {
            var notifications = _context.Notifications
                .AsNoTracking()
                .Include(n => n.Level)
                .Include(n => n.Type)
                .OrderByDescending(n => n.PushTime)
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Name = n.Name,
                    Description = n.Description,
                    PushTime = n.PushTime,
                    LevelId = n.LevelId,
                    LevelName = n.Level.Name,
                    TypeId = n.TypeId,
                    TypeName = n.Type.Name,
                });

            int totalCount = notifications.Count();

            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            notifications = notifications.Skip((page - 1) * pageSize).Take(pageSize);

            var dto = new AllNotificationPageDto
            {
                Notifications = notifications.ToList(),
                TotalPages = totalPages,
            };

            return dto;
        }


        public IQueryable<SendedNotificationDto> GetLatestNotifications(int id)
        {
            var dto = _context.SendedNotifications
                .AsNoTracking()
                .Where(sn => id == sn.MemberId)
                .Include(sn => sn.Notification)
                .OrderByDescending(n => n.Notification.PushTime)
                .Take(3)
                .Select(sn => new SendedNotificationDto
                {
                    MemberId = sn.MemberId,
                    NotificationId = sn.NotificationId,
                    NotificationTitle = sn.Notification.Name,
                    NotificationDescription = sn.Notification.Description,
                    PushTime = sn.Notification.PushTime,
                    Image = sn.Notification.Image,
                });

            return dto;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id">會員Id</param>
        /// <param name="page">顯示第幾頁</param>
        /// <param name="pageSize">每頁幾筆資料</param>
        /// <returns></returns>
        public NotificationPagesDto GetAllNotifications(int id, int page = 1, int pageSize = 10)
        {
            // 選出該會員所有通知
            var notifications = _context.SendedNotifications
                .Where(sn => id == sn.MemberId)
                .Include(sn => sn.Notification)
                .OrderByDescending(sn => sn.Notification.PushTime)
                .Select(sn => new SendedNotificationDto
                {
                    MemberId = sn.MemberId,
                    NotificationId = sn.NotificationId,
                    NotificationTitle = sn.Notification.Name,
                    NotificationDescription = sn.Notification.Description,
                    PushTime = sn.Notification.PushTime,
                    Image = sn.Notification.Image,
                });

            // 取出總共幾筆通知
            int totalCount = notifications.Count();

            // 總頁數
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            notifications = notifications.Skip(pageSize * (page - 1)).Take(pageSize);

            NotificationPagesDto dto = new NotificationPagesDto
            {
                Notiftications = notifications.ToList(),
                TotalPages = totalPages,
            };

            return dto;
        }

        public IQueryable<MemberLevel> GetLevels()
        {

            var dto = _context.MemberLevels
                .AsNoTracking()
                .Select(level => new MemberLevel
                {
                    Id = level.Id,
                    Name = level.Name,
                });

            return dto;
        }

        public IQueryable<NotificationType> GetTypes()
        {
            var dto = _context.NotificationTypes
                .AsNoTracking()
                .Select(type => new NotificationType
                {
                    Id = type.Id,
                    Name = type.Name,
                });

            return dto;
        }

        public IQueryable<NotificationDto> GetNotification(int id)
        {
            var dto = _context.Notifications
                .AsNoTracking()
                .Where(n => n.Id == id)
                .Include(n => n.Level)
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    Name = n.Name,
                    Description = n.Description,
                    PushTime = n.PushTime,
                    Image = n.Image,
                    LevelId = n.LevelId,
                    LevelName = n.Level.Name
                });

            return dto;
        }

        public async Task<string> Edit(NotificationDto dto)
        { 
                var notiModel = _context.Notifications.Find(dto.Id);

                notiModel.Name = dto.Name;
                notiModel.Description = dto.Description;
                notiModel.PushTime = dto.PushTime;
                notiModel.TypeId = dto.TypeId;

                await _context.SaveChangesAsync();

                return "編輯成功"; 
        }

        public async Task<string> Create(SendedNotificationDto dto)
        {
            var notiModel = new Notification
            {
               Name = dto.NotificationTitle,
               Description = dto.NotificationDescription,
               PushTime = dto.PushTime,
               Image = dto.Image,
               LevelId = dto.LevelId,
               TypeId = dto.TypeId,
            };

            _context.Notifications.Add(notiModel);
            await _context.SaveChangesAsync();

            int count = _context.Members.Count();
            var memberId = _context.Members.Select(x => x.Id).ToList();

            if (dto.LevelId != null)
            {
                count = _context.Members
                      .Where(x => x.LevelId == dto.LevelId)
                      .Count();

                memberId = _context.Members
                    .Where(x => x.LevelId == dto.LevelId)
                    .Select(x => x.Id)
                    .ToList();
            }

            for (int i = 0; i < count; i++)
            {
                var snModel = new SendedNotification
                {
                    NotificationId = notiModel.Id,
                    MemberId = memberId[i],
                    IsSended = false
                };

                _context.SendedNotifications.Add(snModel);
            }

            await _context.SaveChangesAsync();

            return "新增成功";
        }
    }
}

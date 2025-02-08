namespace ChatsWebApi.Components.Types.Roles
{
    public interface IAdminLogs
    {
        public AdminLog[] AdminLogsList { get; init; }
        public bool IsAdmin(AdminLog adminLog);
    }
}

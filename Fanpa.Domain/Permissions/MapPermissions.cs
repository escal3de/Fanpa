using System.Collections.ObjectModel;

namespace Fanpa.Domain.Permissions;

public class MapPermissions
{
    public static readonly IReadOnlyDictionary<UserRole, IReadOnlyCollection<string>> Map =
        new ReadOnlyDictionary<UserRole, IReadOnlyCollection<string>>(
            new Dictionary<UserRole, IReadOnlyCollection<string>>
            {
                [UserRole.Client] =
                [
                    Permissions.UsersRead,

                    Permissions.LotsAdd,
                    Permissions.LotsRead
                ],

                [UserRole.Support] =
                [
                    Permissions.UsersBan,

                    Permissions.LotsDelete
                ],

                [UserRole.Admin] =
                [
                    Permissions.UsersDelete,
                    Permissions.UsersUpdate,
                    
                    Permissions.LotsEdit
                ]
            });
}
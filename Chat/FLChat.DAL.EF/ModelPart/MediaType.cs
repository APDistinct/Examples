using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLChat.DAL.Model
{
    public partial class MediaType
    {
        public MediaGroupKind Kind {
            get => (MediaGroupKind)MediaTypeGroupId;
            set => MediaTypeGroupId = (int)value;
        }
    }

    public static class MediaTypeExtentions
    {
        public static MediaType FindOrCreateMediaType(this ChatEntities entities, string mediaType, MediaGroupKind kind, bool onlyEnabled = true) {
            // mediaTypeGroupId
            MediaType mt = entities.FindMediaType(mediaType, onlyEnabled: false);
            if (mt == null) { 
                mt = new MediaType() {
                    Name = mediaType,
                    CanBeAvatar = false,
                    Kind = kind,
                    Enabled = true,
                };
                entities.MediaType.Add(mt);
            }
            if (mt.Enabled == false && onlyEnabled)
                mt = null;
            return mt;
        }

        /// <summary>
        /// Поиск кода mime-типа и группы по наименованию
        /// </summary>
        /// <param name="entities">Контекст</param>
        /// <param name="fileMimeType">mime-тип от клиента</param>
        /// <returns>Найден или нет</returns>
        public static MediaType FindMediaType(this ChatEntities entities, string fileMimeType, bool onlyEnabled = true) {
            return entities
                .MediaType
                .Where(x => x.Name == fileMimeType && (x.Enabled || onlyEnabled == false))
                .FirstOrDefault();
        }
    }
}

using UnityEngine;
using System.Collections.Generic;
using System;
using AYellowpaper.SerializedCollections;

namespace EternalFlame
{
    // Quản lý tất cả các bubble trong trò chơi
    public class BubbleManager : MonoBehaviour
    {
        // Danh sách để lưu trữ tất cả các đối tượng bubble
        private List<Bubble> bubbles = new List<Bubble>();

        // Phương thức tạo bubble dựa trên loại và thông tin truyền vào
        public void CreateBubble<T>(BubbleType type, string text, Vector3 position) where T : Bubble
        {
            // Sử dụng BubbleFactory để tạo bubble dựa trên loại
            Bubble bubble = BubbleFactory.CreateBubble<T>(type, text, position);

            // Thêm bubble vào danh sách quản lý
            bubbles.Add(bubble);

            // Hiển thị bubble với các tham số bổ sung (hiện tại là Dictionary rỗng)
            bubble.Display(new Dictionary<string, string>());
        }

        // Xóa tất cả các bubble khỏi danh sách
        public void ClearBubbles()
        {
            bubbles.Clear();
        }
    }

    // Lớp cơ sở cho tất cả các loại bubble
    public abstract class Bubble
    {
        public string Text { get; set; } // Nội dung văn bản hiển thị trong bubble
        public Vector3 Position { get; set; } // Vị trí của bubble trên màn hình/cảnh

        // Phương thức hiển thị bubble, cần được override trong các lớp con
        public abstract void Display(Dictionary<string, string> parameters);
    }

    // Factory chịu trách nhiệm tạo các đối tượng bubble
    public class BubbleFactory
    {
        // Dictionary ánh xạ loại bubble (BubbleType) với phương thức tạo bubble tương ứng
        private static readonly Dictionary<BubbleType, Func<string, Vector3, Bubble>> factoryMethods =
            new Dictionary<BubbleType, Func<string, Vector3, Bubble>>()
            {
                // Tạo DialogueBubble
                { BubbleType.Dialogue, (text, position) => new DialogueBubble { Text = text, Position = position } },

                // Tạo SkillBubble
                { BubbleType.Skill, (text, position) => new SkillBubble { Text = text, Position = position } },

                // Tạo GuideBubble
                { BubbleType.Guide, (text, position) => new GuideBubble { Text = text, Position = position } }
            };

        // Phương thức tạo bubble chung, sử dụng enum để xác định loại bubble
        public static T CreateBubble<T>(BubbleType type, string text, Vector3 position) where T : Bubble
        {
            // Nếu loại bubble có trong dictionary, gọi phương thức tạo
            if (factoryMethods.TryGetValue(type, out var createMethod))
            {
                return createMethod(text, position) as T; // Trả về bubble tương ứng
            }
            // Nếu loại không hợp lệ, ném ra ngoại lệ
            throw new ArgumentException("Loại bubble không hợp lệ");
        }
    }

    // Bubble cho hội thoại
    public class DialogueBubble : Bubble
    {
        // Hiển thị nội dung hội thoại
        public override void Display(Dictionary<string, string> parameters)
        {
            Debug.Log("Hiển thị hội thoại: " + Text);
        }
    }

    // Bubble cho tên chiêu thức
    public class SkillBubble : Bubble
    {
        // Hiển thị nội dung tên chiêu thức
        public override void Display(Dictionary<string, string> parameters)
        {
            Debug.Log("Hiển thị tên chiêu thức: " + Text);
        }
    }

    // Bubble cho hướng dẫn
    public class GuideBubble : Bubble
    {
        // Hiển thị nội dung hướng dẫn
        public override void Display(Dictionary<string, string> parameters)
        {
            Debug.Log("Hiển thị hướng dẫn: " + Text);
        }
    }

    // Enum định nghĩa các loại bubble
    public enum BubbleType
    {
        Dialogue, // Loại bubble cho hội thoại
        Skill,    // Loại bubble cho chiêu thức
        Guide     // Loại bubble cho hướng dẫn
    }
}

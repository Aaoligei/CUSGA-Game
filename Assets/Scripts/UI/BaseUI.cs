using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Game.Managers;

namespace Game.UI
{
    /// <summary>
    /// UI基类，所有UI界面都应继承此类
    /// </summary>
    public abstract class BaseUI : MonoBehaviour
    {
        // UI类型
        [SerializeField] protected UIType _uiType = UIType.None;
        public UIType UIType => _uiType;
        
        // 界面是否已初始化
        protected bool _isInitialized = false;
        
        // 按钮缓存字典
        protected Dictionary<string, Button> _buttons = new Dictionary<string, Button>();
        
        // 文本缓存字典
        protected Dictionary<string, Text> _texts = new Dictionary<string, Text>();
        
        // 图片缓存字典
        protected Dictionary<string, Image> _images = new Dictionary<string, Image>();
        
        // 输入框缓存字典
        protected Dictionary<string, InputField> _inputFields = new Dictionary<string, InputField>();
        
        // 滚动视图缓存字典
        protected Dictionary<string, ScrollRect> _scrollRects = new Dictionary<string, ScrollRect>();
        
        // 切换按钮缓存字典
        protected Dictionary<string, Toggle> _toggles = new Dictionary<string, Toggle>();

        /// <summary>
        /// 初始化界面
        /// </summary>
        /// <param name="uiType">UI类型</param>
        public virtual void Init(UIType uiType)
        {
            if (_isInitialized)
            {
                return;
            }
            
            _uiType = uiType;
            _isInitialized = true;
            
            RegisterUIComponents();
            OnInit();
        }

        /// <summary>
        /// 界面初始化回调，子类可重写
        /// </summary>
        protected virtual void OnInit() { }

        /// <summary>
        /// 界面显示回调，子类可重写
        /// </summary>
        public virtual void OnShow() { }

        /// <summary>
        /// 界面隐藏回调，子类可重写
        /// </summary>
        public virtual void OnHide() { }

        /// <summary>
        /// 界面销毁回调，子类可重写
        /// </summary>
        public virtual void OnDestroy() { }

        /// <summary>
        /// 注册UI组件
        /// </summary>
        protected virtual void RegisterUIComponents()
        {
            // 注册按钮
            Button[] buttons = GetComponentsInChildren<Button>(true);
            foreach (Button btn in buttons)
            {
                if (!string.IsNullOrEmpty(btn.name))
                {
                    _buttons[btn.name] = btn;
                }
            }
            
            // 注册文本
            Text[] texts = GetComponentsInChildren<Text>(true);
            foreach (Text txt in texts)
            {
                if (!string.IsNullOrEmpty(txt.name))
                {
                    _texts[txt.name] = txt;
                }
            }
            
            // 注册图片
            Image[] images = GetComponentsInChildren<Image>(true);
            foreach (Image img in images)
            {
                if (!string.IsNullOrEmpty(img.name))
                {
                    _images[img.name] = img;
                }
            }
            
            // 注册输入框
            InputField[] inputFields = GetComponentsInChildren<InputField>(true);
            foreach (InputField input in inputFields)
            {
                if (!string.IsNullOrEmpty(input.name))
                {
                    _inputFields[input.name] = input;
                }
            }
            
            // 注册滚动视图
            ScrollRect[] scrollRects = GetComponentsInChildren<ScrollRect>(true);
            foreach (ScrollRect scroll in scrollRects)
            {
                if (!string.IsNullOrEmpty(scroll.name))
                {
                    _scrollRects[scroll.name] = scroll;
                }
            }
            
            // 注册切换按钮
            Toggle[] toggles = GetComponentsInChildren<Toggle>(true);
            foreach (Toggle toggle in toggles)
            {
                if (!string.IsNullOrEmpty(toggle.name))
                {
                    _toggles[toggle.name] = toggle;
                }
            }
        }

        /// <summary>
        /// 获取按钮
        /// </summary>
        /// <param name="name">按钮名称</param>
        /// <returns>按钮组件</returns>
        protected Button GetButton(string name)
        {
            if (_buttons.TryGetValue(name, out Button btn))
            {
                return btn;
            }
            
            // 未找到缓存，尝试查找
            Button findBtn = transform.Find(name)?.GetComponent<Button>();
            if (findBtn != null)
            {
                _buttons[name] = findBtn;
                return findBtn;
            }
            
            Debug.LogWarning($"未找到按钮: {name}");
            return null;
        }

        /// <summary>
        /// 获取文本
        /// </summary>
        /// <param name="name">文本名称</param>
        /// <returns>文本组件</returns>
        protected Text GetText(string name)
        {
            if (_texts.TryGetValue(name, out Text txt))
            {
                return txt;
            }
            
            // 未找到缓存，尝试查找
            Text findTxt = transform.Find(name)?.GetComponent<Text>();
            if (findTxt != null)
            {
                _texts[name] = findTxt;
                return findTxt;
            }
            
            Debug.LogWarning($"未找到文本: {name}");
            return null;
        }

        /// <summary>
        /// 获取图片
        /// </summary>
        /// <param name="name">图片名称</param>
        /// <returns>图片组件</returns>
        protected Image GetImage(string name)
        {
            if (_images.TryGetValue(name, out Image img))
            {
                return img;
            }
            
            // 未找到缓存，尝试查找
            Image findImg = transform.Find(name)?.GetComponent<Image>();
            if (findImg != null)
            {
                _images[name] = findImg;
                return findImg;
            }
            
            Debug.LogWarning($"未找到图片: {name}");
            return null;
        }

        /// <summary>
        /// 获取输入框
        /// </summary>
        /// <param name="name">输入框名称</param>
        /// <returns>输入框组件</returns>
        protected InputField GetInputField(string name)
        {
            if (_inputFields.TryGetValue(name, out InputField input))
            {
                return input;
            }
            
            // 未找到缓存，尝试查找
            InputField findInput = transform.Find(name)?.GetComponent<InputField>();
            if (findInput != null)
            {
                _inputFields[name] = findInput;
                return findInput;
            }
            
            Debug.LogWarning($"未找到输入框: {name}");
            return null;
        }

        /// <summary>
        /// 获取滚动视图
        /// </summary>
        /// <param name="name">滚动视图名称</param>
        /// <returns>滚动视图组件</returns>
        protected ScrollRect GetScrollRect(string name)
        {
            if (_scrollRects.TryGetValue(name, out ScrollRect scroll))
            {
                return scroll;
            }
            
            // 未找到缓存，尝试查找
            ScrollRect findScroll = transform.Find(name)?.GetComponent<ScrollRect>();
            if (findScroll != null)
            {
                _scrollRects[name] = findScroll;
                return findScroll;
            }
            
            Debug.LogWarning($"未找到滚动视图: {name}");
            return null;
        }

        /// <summary>
        /// 获取切换按钮
        /// </summary>
        /// <param name="name">切换按钮名称</param>
        /// <returns>切换按钮组件</returns>
        protected Toggle GetToggle(string name)
        {
            if (_toggles.TryGetValue(name, out Toggle toggle))
            {
                return toggle;
            }
            
            // 未找到缓存，尝试查找
            Toggle findToggle = transform.Find(name)?.GetComponent<Toggle>();
            if (findToggle != null)
            {
                _toggles[name] = findToggle;
                return findToggle;
            }
            
            Debug.LogWarning($"未找到切换按钮: {name}");
            return null;
        }

        /// <summary>
        /// 添加按钮点击事件
        /// </summary>
        /// <param name="name">按钮名称</param>
        /// <param name="callback">点击回调</param>
        protected void AddButtonClickListener(string name, UnityEngine.Events.UnityAction callback)
        {
            Button btn = GetButton(name);
            if (btn != null)
            {
                btn.onClick.AddListener(callback);
            }
        }

        /// <summary>
        /// 移除按钮点击事件
        /// </summary>
        /// <param name="name">按钮名称</param>
        /// <param name="callback">点击回调</param>
        protected void RemoveButtonClickListener(string name, UnityEngine.Events.UnityAction callback)
        {
            Button btn = GetButton(name);
            if (btn != null)
            {
                btn.onClick.RemoveListener(callback);
            }
        }

        /// <summary>
        /// 添加切换按钮值变化事件
        /// </summary>
        /// <param name="name">切换按钮名称</param>
        /// <param name="callback">值变化回调</param>
        protected void AddToggleValueChangedListener(string name, UnityEngine.Events.UnityAction<bool> callback)
        {
            Toggle toggle = GetToggle(name);
            if (toggle != null)
            {
                toggle.onValueChanged.AddListener(callback);
            }
        }

        /// <summary>
        /// 移除切换按钮值变化事件
        /// </summary>
        /// <param name="name">切换按钮名称</param>
        /// <param name="callback">值变化回调</param>
        protected void RemoveToggleValueChangedListener(string name, UnityEngine.Events.UnityAction<bool> callback)
        {
            Toggle toggle = GetToggle(name);
            if (toggle != null)
            {
                toggle.onValueChanged.RemoveListener(callback);
            }
        }

        /// <summary>
        /// 设置文本内容
        /// </summary>
        /// <param name="name">文本名称</param>
        /// <param name="content">文本内容</param>
        protected void SetText(string name, string content)
        {
            Text txt = GetText(name);
            if (txt != null)
            {
                txt.text = content;
            }
        }

        /// <summary>
        /// 设置图片
        /// </summary>
        /// <param name="name">图片名称</param>
        /// <param name="sprite">图片精灵</param>
        protected void SetImage(string name, Sprite sprite)
        {
            Image img = GetImage(name);
            if (img != null)
            {
                img.sprite = sprite;
            }
        }

        /// <summary>
        /// 设置输入框内容
        /// </summary>
        /// <param name="name">输入框名称</param>
        /// <param name="content">输入内容</param>
        protected void SetInputField(string name, string content)
        {
            InputField input = GetInputField(name);
            if (input != null)
            {
                input.text = content;
            }
        }

        /// <summary>
        /// 设置切换按钮状态
        /// </summary>
        /// <param name="name">切换按钮名称</param>
        /// <param name="isOn">是否开启</param>
        protected void SetToggle(string name, bool isOn)
        {
            Toggle toggle = GetToggle(name);
            if (toggle != null)
            {
                toggle.isOn = isOn;
            }
        }

        /// <summary>
        /// 关闭当前界面
        /// </summary>
        protected void CloseUI()
        {
            UIManager.Instance.CloseUI(_uiType);
        }
    }
} 
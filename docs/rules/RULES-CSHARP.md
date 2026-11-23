# C# / .NET 规则

## 通用约束

1. 业务逻辑必须放在 Service 层；ViewModel 只负责界面状态与命令，通过调用 Service 完成业务。
2. 不滥用扩展方法。
3. 不使用 `dynamic`。
4. 避免 `static` 可变状态。
5. 尽量使用依赖注入，不手动 `new` 依赖，除非之前也是这么干的。
6. 尽量不修改现有命名空间结构。
7. 严格遵守 `public`/`internal`/`private` 规则，规则如下：
    - 使用"公共"一词来表示"此成员不是实现细节"。
    - 受保护的成员是实现细节；它包含一些派生类正常运行所必需的信息。
    - 内部成员也是实现细节；程序集内部的其他部分需要该成员才能正常工作。
    - 公共成员则表示"此成员代表此对象提供的关键且已记录的功能"。
8. WinUI 3（MVVM）：
    - View 的 code-behind 只处理纯 UI 行为（如动画、焦点、视觉状态切换），不包含业务逻辑，也不直接调用 Service。
    - ViewModel 不直接调用 Win32 / PInvoke / 文件系统等平台 API，通过注入的服务访问。
    - 后台线程更新 UI 必须经由 `DispatcherQueue`，不得在非 UI 线程直接访问 UI 对象。
    - 系统钩子（如低级鼠标 / 键盘钩子）回调中不得执行耗时或阻塞操作。

## C# 代码风格规范（个人版本）

### 零、配置文件读取规则

1. 每次修改或新增 C# 代码前，读取项目根目录的 `.editorconfig`。
2. `.editorconfig` 中标记为 `:warning` 的规则（含 `dotnet_naming_rule.*.severity = warning` 的命名规则）视同 `:error`，必须严格遵守。
3. `.editorconfig` 中无严重性标记的规则（suggestion 或无标注），在不违反本文档其他规则的前提下，以代码优雅性为准自行判断是否遵从。
4. 本文档（手写规则）与 `.editorconfig` 有冲突时，**以本文档为准**。

### 一、风格

1. 仅当右侧显式写出类型时使用 `var`（`new` 表达式或显式类型转换），例如 `var stream = new FileStream(...)`，而非 `var stream = OpenStandardInput()`；
    - 内置类型（`int`/`string`/`bool` 等）使用显式类型，例如 `int count = 0;`。
    - 方法返回值（包括接口返回）必须显式类型。
    - 接口类型一定要显式声明，即使右侧 `new` 与类型一致，防止误用 `var`。
    - 不要使用隐式类型化来确定 `foreach` 循环中循环变量的类型。
    - 对 LINQ 查询中的结果序列使用隐式类型。
2. 所有成员必须显式指定访问修饰符（`public`/`private`/`internal`）。
3. 方法长度控制在50行以内，如果方法复杂度高且业务逻辑清晰，可以适当拆分辅助私有方法，而不强制拆分。
4. 大括号必须换行（Allman 风格）。
5. 所有 `if` 语句必须使用大括号，禁止省略大括号的单行 `if`，即使逻辑简单。
6. 禁止空的 `catch` 块。
7. 禁止嵌套三元表达式。
8. 返回值必须先赋给局部变量，再 `return`，以便调试和后续扩展。
    - 示例：

        ```csharp
        var items = GetItems();        // 推荐，便于调试或后续操作
        return items;
        ```

    - 禁止直接 `return` 方法调用或函数执行结果（包括大部分 LINQ 方法链）：

        ```csharp
        return GetItems();             // 不允许
        return Calculate();            // 不允许
        return myList.Where(x => x > 0); // 不允许直接 return 查询结果
        return from x in myList
               where x > 0
               select x;              // 不允许直接 return 查询语法
        ```

    - 允许对局部变量执行最终转换（例如 `ToList()`）后 `return`：

        ```csharp
        var query = myList.Where(x => x > 0);
        return query.ToList();         // 允许
        return GetItems().ToList();    // 不允许：没有中间变量，调试不可行
        ```

    - 允许简单计算或逻辑表达式直接 `return`（仅限非函数调用、非 LINQ 方法链）：

        ```csharp
        return a + b;                  // 允许
        return a > b ? x : y;          // 允许
        ```

    - FP 风格例外（Result / Option Fluent 链式调用）：
        - 使用 `Result<T,E>`、`Option<T>`、LINQ pipeline（参见六.5）或其他具名 Fluent API（如 Builder 模式）时，允许直接在局部变量上进行 Fluent 链式调用后返回，无需为每个中间步骤单独赋变量。
        - 初始值必须先赋给局部变量；链式调用超过 3 层时必须换行，每行只保留一个调用，风格与 F# 的 `|>` 管道规范对齐：

            ```csharp
            var result = await _repository.GetByIdAsync(id, ct);  // 初始值赋变量
            return result
                .Map(entity => _mapper.ToDto(entity))
                .MapError(_ => DomainError.NotFound);             // ✅ 允许
            ```

        - 禁止省略初始变量：

            ```csharp
            return (await _repository.GetByIdAsync(id, ct))       // ❌ 初始值未赋变量
                .Map(...);
            ```

    - `localVar.DistinctBy(...).ToList()` / `localVar.OrderBy(...).ToList()` 等"收口前的单次整理操作"视为最终转换，允许直接 `return`。
9. 返回 `Task` / `ValueTask` 等可等待类型的异步方法必须以 `Async` 结尾；`async void` 事件处理程序（如 `OnStartUpToggled`）与框架重写方法（如 `OnNavigatedTo`）不加 `Async` 后缀。
10. 对于 `async Task` 方法，必须正确 `await`，避免未观察异常。
11. 禁止 `async void`。
    - 唯一例外：WinUI 事件处理程序（如 `Click`、`Toggled`）以及框架要求返回 `void` 的重写方法（如 `OnNavigatedTo`、`OnLaunched`）。
    - 例外情况下方法体必须用 `try-catch` 包住全部 `await` 调用并处理异常（记录日志或提示用户），不得让异常逃逸到调用方。
    - 例外方法内只做调用转发，实际逻辑放在返回 `Task` 的 `Async` 方法中（例如 `await ViewModel.InitializeAsync();`）。
12. `Task.Result`/`Wait()` 必须说明原因，并且尽量提供替代方案（例如 `async`/`await` 链式调用）。
13. 不混合同步与异步调用。
14. 不随意添加 `ConfigureAwait(false)`，除非明确说明。
    - View、ViewModel 等 `await` 之后需要访问 UI 对象或绑定属性的代码，禁止使用 `ConfigureAwait(false)`，否则后续代码可能不在 UI 线程上执行。
15. 所有 `IDisposable` 必须使用 `using` 或显式释放，不允许遗漏。
16. 不自动优化代码（例如禁止 AI 或 IDE 自动 apply 重构/优化）。
17. 不添加不必要注释。
18. 优先使用 C# 关键字形式（比如 `string`, `int`, `float`），而不是 FCL 类型（`String`, `Int32`, `Single`）。
19. 字符串含转义序列时优先使用原始字符串字面量（`"""`）；拼接短字符串用字符串内插，大量文本用 `StringBuilder`。
20. 使用集合表达式初始化所有集合类型。
21. 在执行比较时，使用 `&&` 而不是 `&`，使用 `||` 而不是 `|`。
22. 当变量类型与对象类型匹配时，请使用对象实例化的简明形式之一。如 `Item i = new()` 而不是 `Item i = new Item();`
23. 使用 lambda 表达式定义稍后无需移除的事件处理程序。
24. 代码文件（C#、F#、PowerShell 等）使用四个空格缩进；XAML 及 XML 系文件（csproj/props/targets/slnx/resx/manifest 等）、JSON、YAML 使用两个空格缩进。不要使用制表符。
25. 使用单行注释（`//`）以进行简要说明。避免使用多行注释（`/* */`）来进行较长的解释。
    - 若要描述方法、类、字段和所有公共成员，请使用 XML 注释。
    - 将注释放在单独的行上，而非代码行的末尾。在注释分隔符 (`//`) 与注释文本之间插入一个空格。
26. 每行只写一条语句。每行只写一个声明。
27. 使用括号突出表达式中的子句。

### 二、命名

1. 类名使用 PascalCase。（例如 `public class ItemService`）
2. 方法名使用 PascalCase。（例如 `public Item GetItem() {}`）
3. 属性名使用 PascalCase。（例如 `public Item CurrentItem { get; set; }`）
4. 私有字段使用 `_camelCase`。（例如 `private Item _currentItem;`）
5. 私有/内部静态字段使用 `s_camelCase`（例如 `private static Item s_currentItem;`），线程静态字段（`[ThreadStatic]`）使用 `t_camelCase`。
6. 局部变量使用 camelCase。（例如 `var item = new Item();`）
7. 接口使用 `I` 前缀。（例如 `public interface IItemService`）
8. 常量（字段与局部常量，包括 `private`/`internal`）使用 PascalCase（例如 `private const int ItemCount = 10;`），不使用全大写。
    - 唯一例外：interop 常量需与被调用的原生 API 名称保持一致（例如 `WM_MOUSEWHEEL`）。
9. 不使用缩写，广为人知的缩写除外；缩写的大小写遵循 Framework Design Guidelines：
    - 两个字母的缩写全大写（IO、UI），三个字母及以上只大写首字母（Html、Url、Xml），Id 写作 Id 而非 ID。
10. 名称必须表达语义，不允许 a/b/temp 之类命名。
11. 不重命名现有变量。
12. 参数使用 camelCase；局部函数使用 PascalCase。
13. 泛型类型参数以 `T` 开头（例如 `TItem`）；只有一个类型参数时可直接使用 `T`。
14. 如以上命名规则与项目中的规范冲突，按照项目规范为准

### 三、结构

#### 1. 类成员顺序

类成员按照语义职责分组，并遵循以下顺序。组间使用空行分隔，具有相同或直接关联语义的成员应尽量相邻。

- 常量
- 属性及索引器
    - 公共属性
    - 内部属性
    - 受保护属性
    - 私有属性
    - 仅服务于单个属性的专用后备字段，应紧邻对应属性，并置于属性之前
- 委托及事件
    - 委托
    - 公共事件
    - 内部事件
    - 受保护事件
    - 私有事件
- 依赖字段
    - 注入依赖：由外部提供的协作组件，不限制具体类型及注入方式
    - 内部依赖：由当前类型自行创建或获取的协作组件
- 内部字段
    - 资源字段：由当前类型持有或管理的运行时资源，如同步原语、取消令牌源等
    - 状态字段：用于描述或维护当前类型运行状态的字段，如状态标记、计数器、缓存等
- 构造函数及终结器
    - 静态构造函数
    - 实例构造函数
    - 终结器
- 运算符及转换运算符
- 公共方法
- 内部方法
- 受保护方法
- 私有方法
- 嵌套类型

#### 2. 组内排序

同组成员默认按照以下优先级排序：

1. 访问级别：`public` → `internal` → `protected internal` → `protected` → `private protected` → `private`
2. 静态性：静态成员优先于实例成员
3. 只读性：只读字段优先于可变字段

#### 3. 补充原则

- 语义关联优先于机械排序。相关属性、方法重载、同一接口的实现成员等，应尽量保持相邻。
- 初始化方法（如 `Initialize`、`InitXxx`）在其所属分组内排在最前。
- 字段应根据其语义职责分类，而非依据字段类型、实例化方式或初始化位置。
- 被多个成员共同使用的状态字段，应归入内部字段，而非属性后备字段。
- 成员排序不得改变字段初始化顺序所产生的程序行为。
- 使用空行进行成员分组，无须添加仅用于标识分组的注释或 `#region`。
- 同组成员可以连续书写、不空行；同组内语义差距较大的成员之间空一行。多行成员（方法、带访问器主体的属性等）之间始终空一行。
- 不要求为不存在的成员类别保留空行或占位内容。

#### 4. 其他

1. 构造函数：参数分行，按依赖字段、内部字段、其他参数顺序排列，组间空行。
2. `using` 按根命名空间分组，`System` 开头的组排在最前，组间空一行（对应 `.editorconfig` 的 `dotnet_sort_system_directives_first`、`dotnet_separate_import_directive_groups`）。
3. 每个文件只包含一个公共类型。
4. 禁止 `using static`。
5. 优先使用 `Func<>` 和 `Action<>`，而不是自定义委托类型；仅在需要具名委托以显著提高可读性时才定义自定义委托类型。创建委托实例时使用 lambda 简洁语法。
6. 将 `using` 指令放在命名空间声明之外。

### 四、异常/日志

1. 只捕获可处理的异常。
2. 禁止吞异常。
3. 业务异常必须明确类型。
4. 不使用 `Exception` 作为通用异常（允许捕获 `Exception` 做日志或 wrap，但不能吞掉）。
5. 不为每个方法添加 `try-catch`。
6. 不在底层重复记录日志。

### 五、接口/类

1. 不创建无意义的接口（接口是否代表一个可替换的领域行为/业务/事实行为）。
2. 不使用静态工具类承载业务逻辑。
3. 避免 God Class，单个类方法数量尽量不超过 20 个。
4. 构造函数只做依赖注入，不写业务逻辑。XAML 类型（`Window`、`Page`、`UserControl`）构造函数中的 `InitializeComponent()` 调用不受此限。
5. 仅当提高可读性时才拆分方法，不为形式拆分。
6. 不将简单类拆分成多个文件。

### 六、LINQ

1. 在提高可读性时使用 LINQ。
2. 禁止嵌套超过两层 LINQ。
3. 不在高频路径中使用复杂 LINQ。
4. 禁止在循环中反复执行 LINQ 查询。
5. 不使用过度链式调用。对 C# LINQ，建议链式调用不超过 3-4 层；Result/Option 等 FP Fluent 链及 LINQ linear pipeline（见例外）不受此限。
    - 例外：LINQ 作为线性函数式 pipeline 使用时（每步为独立变换，无嵌套，以 `.ToArray()`/`.ToList()` 等收口），层数可超过 4 层，以可读性为准。
6. 对延迟执行查询（`IEnumerable`）注意高频访问性能，可提前 `ToList()`/`ToArray()`。
7. 对查询变量使用有意义的名称。使用别名确保匿名类型的属性名称都使用 Pascal 大小写格式正确大写。
8. 尽量使用 Immutable 特性，例如 `IReadOnlyList<T>`

### 七、现代 C# 特性

1. 可使用现代 C# 特性（包括不限于 switch 表达式、模式匹配、record 类型、expression-bodied members、async/await、LINQ、async enumerable），但需注意：
    - 可读性和调试
    - 性能影响
    - 延迟执行和异常处理（尤其是 `IAsyncEnumerable` 和 LINQ 延迟查询）
    - record 类型的不可变性
    - 对于 LINQ 和 async/await，仍需遵循六、LINQ 中的性能和可读性要求，以及风格一节中对异步方法的要求。
    - 对于现代特性，也要遵循上边提出的各项原则。
2. 可以使用（也推荐使用）函数式编程特性，但需注意可读性和调试。
3. DTO 和值对象尽量使用 `required` 属性而不是构造函数来强制初始化属性值。
4. FP 风格优先（Result / Option 链）：在 FP 风格或 FP like 代码中，优先使用 Fluent API 链式调用（`.Map`、`.Bind`、`.MapError`、`.Tap`、`.Match` 等）而非 `if`/`else` 展开；格式规范参照 F# 管道规则：
    - 超过 3 层时换行，每行保留一个方法调用。
    - 如果项目中没有引用 FP 库，使用 Linq 作为函数式库

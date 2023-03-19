using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperBox.Database.Configuration;

/// <summary>
///     Элемент конфигурации
/// </summary>
internal interface IConfigItem
{
    /// <summary>
    ///     Установить конфигурацию
    /// </summary>
    void Setup(ModelBuilder modelBuilder);
}

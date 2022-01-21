namespace back_end.Utilidades
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;

    public class TypeBinder<T> : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            string nombrePropiedad = bindingContext.ModelName;
            ValueProviderResult valor = bindingContext.ValueProvider.GetValue(nombrePropiedad);
            if (valor == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }
            try
            {
                var valorDeserializado = JsonConvert.DeserializeObject<T>(valor.FirstValue);
                bindingContext.Result = ModelBindingResult.Success(valorDeserializado);
            }
            catch
            {
                bindingContext.ModelState.TryAddModelError(nombrePropiedad, Constants.messages.valorNoAdecuado);
            }
            return Task.CompletedTask;
        }
    }
}

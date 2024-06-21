описание задачи

структура данных:

![alt text](image.png)

примерный вид того что хочется сделать

![alt text](image-1.png)

###### 2024-06-20
---
последовательность отрисовки странички  
[11:00:12 WRN] IndexModel.OnGet  
[11:00:13 WRN] _ViewStart.cshtml  
[11:00:13 WRN] index.cshtml  
[11:00:24 WRN] _layout.cshtml  

###### 2024-06-21
---

чтобы работал обработчик post запросом OnPost в моделе IndexModel  
```cs
    public IActionResult OnPost(string? FirstName, string? LastName){  
```        
мне понадобилось внести следующие изменения:    
- в начало cshtml страницы или в _ViewImports.cshtml вставить:  
```html
	@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers  
```    
- в	cshtml страничке:  
```html
	@* пример работы с post формой *@  
	<form method="post" style="margin-bottom:20px">  
		<div><input type="text" name="FirstName" value="aa111aa"/></div>  
		<div><input type="text" name="LastName" value="bb222bb"/>  
			<input type="submit" />  
		</div>  
	</form>   
```    
применённый тут метод, с одной стороны довольно удобен и прост, 
но мне кажется, подходит только для небольших, простых страниц. Т.к. 
при всяком изменении (например выбран другой покупатель) 
идёт запрос на сервер и обновляется вся страничка.  



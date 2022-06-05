// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const carrito = document.querySelector('#carrito');
const lista = document.querySelector("#lista-cursos");
const contenedor = document.querySelector('#body');

let articulosCarrito = [];


 cargarEventListeners();
 function cargarEventListeners(){

    document.addEventListener('DOMContentLoaded' , () => {
       
        callApiListItems();

    });
  
    
    carrito.addEventListener('click' , eliminarCurso);

 }

 async function verId(id){
      
     const mensaje = await callApiReviewProduct(id);
     console.log(mensaje);
     
      if(mensaje == ""){

        errorSweetAlert('Lo siento...' , 'Debes iniciar sesion para agreagar un curso al carrito');

      }else if(mensaje == "El curso ya esta en el carrito"){

        errorSweetAlert('Lo siento...' , 'El curso que quieres añadir ya esta en tu carrito de compras');

      }else if(mensaje == "Curso agregado al carrito"){

         Swal.fire({
            icon: 'success',
            title: 'El curso fue agregado al carrito satisfactoriamente',
          })

          callApiListItems();


      }else if(mensaje == "Curso no encontrado"){

        errorSweetAlert('Lo siento...' , 'El curso que quieres añadir no existe');

      }else if(mensaje == "El usuario ya esta inscrito en el curso"){

        errorSweetAlert('Lo siento...' , 'El curso no se puede añadir al carrito porque ya estas inscrito en el curso');

      }else {
           
        errorSweetAlert('Opps...' , 'Algo salio mal vuelve a intentarlo mas tarde');

      }

    
}

async function mensajeTarjeta(nombreTarjeta, numeroTarjeta, duedate, cvv, nombreTitular, monto){

    const mensaje = await validarTarjeta(nombreTarjeta, numeroTarjeta, duedate, cvv, nombreTitular, monto);
     console.log(mensaje);
    
    if(mensaje == 'Los datos de la tarjeta son correctos.'){
        Swal.fire({
            title: '¿Estas seguro de realizar el pago?',
            text: "Recuerda que no hay metodo de recuperacion una vez haya adquirido los curso!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Si, realizar pago!'
          }).then((result) => {
            if (result.value) {
              
                actualizarSaldoTarjeta(numeroTarjeta, monto);

                Swal.fire(
                'Pago realizado!',
                'El pago se realizo con exito',
                'success'
              )
              form.submit();
            }
          })
    }else{
        errorSweetAlert('Error' , mensaje);
    }

}

async function validarTarjeta(nombreTarjeta, numeroTarjeta, duedate, cvv, nombreTitular, monto){
    let mensaje = "";
    var url = `https://localhost:5001/api/tarjeta?nombreTarjeta=${nombreTarjeta}&numeroTarjeta=${numeroTarjeta}&duedate=${duedate}&cvv=${cvv}&nombreTitular=${nombreTitular}&monto=${monto}`;

    const response = await fetch(url , {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    });

    if(response.ok){

        data = await response.json()
        const {mensajeTarjeta} = data;
        mensaje = mensajeTarjeta;

    }else {

        console.error(response.status);

    }

    return mensaje;
}

async function actualizarSaldoTarjeta(numeroTarjeta, monto){
    let mensaje = "";
    var url = `https://localhost:5001/api/tarjeta?numeroTarjeta=${numeroTarjeta}&monto=${monto}`;

    const response = await fetch(url , {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    });

    if(response.ok){

        data = await response.json()
        const {mensajeSaldo} = data;
        mensaje = mensajeSaldo;

    }else {

        console.error(response.status);

    }

    return mensaje;
}

function consultDelete(id){
  
    Swal.fire({
        title: 'Estas seguro de eliminar?',
        text: "Recuerda que se va a eliminar de forma permanente!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Si, eliminar!'
      }).then((result) => {
        if (result.isConfirmed) {

            eliminarProforma(id);

        }
      })

}

async function eliminarProforma(id){
  
     const mensaje = await callApiDeleteProforma(id);

        if(mensaje == "Curso eliminado del carrito"){
                
                Swal.fire({
                    icon: 'success',
                    title: 'El curso fue eliminado del carrito satisfactoriamente',
                })
    
                callApiListItems();
               
                setTimeout(() => {
                     
                    location.reload();
                    
                }, 1000);
    
        }else if(mensaje == "Curso no encontrado"){
                                    
                errorSweetAlert('Lo siento...' , 'El curso que quieres eliminar no existe');
            
        }else if(mensaje == ""){

            errorSweetAlert('Lo siento...' , 'Debes iniciar sesion para eliminar un curso del carrito');

        }else {

            errorSweetAlert('Opps...' , 'Algo salio mal vuelve a intentarlo mas tarde');

        }

}

async function callApiDeleteProforma(id){

    const url = `https://localhost:5001/api/carrito?id=${id}`;
    let mensaje = "";

    const response = await fetch(url , {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json'
        }
    });

    if(response.ok){

        data = await response.json()
        const {course} = data;
        mensaje = course;

    }else {

        console.error('Error en la peticion');

    }

    return mensaje;

}

function errorSweetAlert(titulo , texto){
  
    Swal.fire({
        icon: 'error',
        title: `${titulo}`,
        text: `${texto}`
      })

}

 async function callApiReviewProduct(id){
    
    let mensaje = "";
    var url = `https://localhost:5001/api/carrito?id=${id}`;

    const response = await fetch(url , {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        }
    });

    if(response.ok){

        data = await response.json()
        const {course} = data;
        mensaje = course;

    }else {

        console.error(response.status);

    }

    return mensaje;
 }

 function callApiListItems(){
      
    const urlFetch = 'https://localhost:5001/api/carrito';
    fetch(urlFetch)
       .then(response => {
           
            if(response.ok){
                return response.json();
            }else {
                   vaciarCarrito();
                   throw new Error('Error en la peticion');   
            }

       })
       .then(data => {
               
               articulosCarrito = data;
               carritoHtml();
       })
       .catch(error => console.log(error));

 }

 function vaciarCarrito(){

     articulosCarrito.splice(0 , articulosCarrito.length);
     carritoHtml();

 }

 function eliminarCurso(e){
   
     if(e.target.classList.contains('borrar-curso')){
  
        const id = e.target.getAttribute('data-id');
        const indice = articulosCarrito.findIndex( prod => prod.id === id);
        articulosCarrito.splice(indice , 1);
        carritoHtml();

        var URLactual = window.location.href;
       
        if(URLactual == 'https://localhost:5001/Carro/MostrarItems'){
            mostrarElementos(articulosCarrito);
        }
    
     }

 }

  
  function carritoHtml(){
    
    limpiarCarrito();
    articulosCarrito.forEach(proforma => {
      
        const row = document.createElement('tr');
    
         row.innerHTML = `

            <td>
              
            <img src="data:image/png;base64,${proforma.curso.fileBase64}" width="100" >
             
            </td>
           
             <td>
              
                 ${proforma.curso.nombre}

             </td>

             <td>
              
                 ${proforma.curso.precio}

             </td>

             <td>
              
               ${proforma.curso.fechaInicio}

         </td>


         `;
    
         contenedor.appendChild(row);
   
    })

   
  }

  function limpiarCarrito(){

      contenedor.innerHTML = '';
  }

  function consultarModalidad(){
      
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
          confirmButton: 'btn btn-success',
          cancelButton: 'btn btn-warning'
        },
        buttonsStyling: false
      })
      
      swalWithBootstrapButtons.fire({
        title: 'Elige tu metodo de pago',
        text: "Puedes pagar con tu tarjeta de credito o con una transferencia desde tu cuenta bancaria o el agente mas cercano",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'PAGAR CON TARJETA',
        cancelButtonText: 'PAGAR CON TRANSFERENCIA',
        reverseButtons: true
      }).then((result) => {
        if (result.isConfirmed) {
            
            window.location.href = 'https://localhost:5001/Pago/IndexPago';

        } else if (
          /* Read more about handling dismissals below */
          result.dismiss === Swal.DismissReason.cancel
        ) {
            
            window.location.href = 'https://localhost:5001/Pago/Transferencia';

        }
      })

  }
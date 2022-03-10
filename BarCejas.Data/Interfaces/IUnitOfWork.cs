using BarCejas.Entities;

using Microsoft.Data.SqlClient;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarCejas.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        #region Interfaces Repository
        IRepository<ContactoLocal> contactoLocalRepository { get; }

        IRepository<Dia> diaRepository { get; }

        IUsuarioRepository usuarioRepository { get; }

        IRepository<Categoria> categoriaRepository { get; }

        IRepository<Servicio> servicioRepository { get; }

        IRepository<Profesional> profesionalRepository { get; }

        IRepository<FormaPago> formaPagoRepository { get; }

        IRepository<ModalidadPago> modalidadPagoRepository { get; }

        IRepository<Paquete> paqueteRepository { get; }

        IRepository<Novedades> NewnessRepository { get; }

        IRepository<PreguntasFrecuentes> FrequentQuestionRepository { get; }

        IRepository<Testimonios> TestimonialRepository { get; }

        IRepository<MediosContactoEmpresa> mediosContactoEmpresaRepository { get; }

        IRepository<Banner> bannerRepository { get; }

        IRepository<HorarioAtencionProfesional> horarioAtencionProfesionalRepository { get; }

        IRepository<CredencialMercadoPago> credencialMercadoPagoRepository { get; }

        IRepository<Orden> ordenRepository { get; }

        IRepository<OrdenItem> ordenItemsRepository { get; }

        IRepository<MensajeMasivo> mensajeMasivoRepository { get; }

        IRepository<ServicioPaquete> servicioPaqueteRepository { get; }

        IRepository<ServicioProfesional> servicioProfesionalRepository { get; }

        IRepository<HorarioBloqueado> horarioBloqueadoRepository { get; }

        IRepository<HistoricoIngresos> historicoIngresosRepository { get; }

        IStoredProcedureRepository reporteProcedureRepository { get; }


        #endregion
        void SaveChange();

        Task SaveChangeAsync();

        // Task<bool> ChangeStatus(string pquery, List<SqlParameter> parameters);
    }
}

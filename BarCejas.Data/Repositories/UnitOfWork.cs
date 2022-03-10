using System;
using BarCejas.Data.DataContext;
using BarCejas.Data.Interfaces;
using BarCejas.Entities;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BarCejas.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BardecejasContext _context;

        private readonly IRepository<ContactoLocal> _contactoLocalRepository;
        private readonly IRepository<Dia> _diaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IRepository<Categoria> _categoriaRepository;
        private readonly IRepository<Servicio> _servicioRepository;
        private readonly IRepository<Profesional> _profesionalRepository;
        private readonly IRepository<HorarioAtencionProfesional> _horarioAtencionProfesionalRepository;
        private readonly IRepository<FormaPago> _formaPagoRepository;
        private readonly IRepository<ModalidadPago> _modalidadPagoRepository;
        private readonly IRepository<Paquete> _paqueteRepository;
        private readonly IRepository<OrdenItem> _ordenItemsRepository;
        private readonly IRepository<Orden> _ordenRepository;
        private readonly IRepository<CredencialMercadoPago> _credencialMercadoPagoRepository;

        private readonly IRepository<Novedades> _newnessRepository;
        private readonly IRepository<PreguntasFrecuentes> _frequentRepository;
        private readonly IRepository<Testimonios> _testimonailRepository;
        private readonly IRepository<MediosContactoEmpresa> _mediosContactoEmpresaRepository;
        private readonly IRepository<Banner> _bannerRepository;

        private readonly IRepository<MensajeMasivo> _mensajeMasivoRepository;

        private readonly IRepository<ServicioPaquete> _servicioPaqueteRepository;
        private readonly IRepository<ServicioProfesional> _servicioProfesionalRepository;
        private readonly IRepository<HorarioBloqueado> _horarioBloqueadoRepository;

        private readonly IRepository<HistoricoIngresos> _historicoIngresosRepository;

        private readonly IStoredProcedureRepository _reporteProcedureRepository;


        public UnitOfWork(BardecejasContext context) => _context = context;

        public IRepository<ContactoLocal> contactoLocalRepository => _contactoLocalRepository ?? new BaseRepository<ContactoLocal>(_context);

        public IRepository<Dia> diaRepository => _diaRepository ?? new BaseRepository<Dia>(_context);

        public IUsuarioRepository usuarioRepository => _usuarioRepository ?? new UsuarioRepository(_context);

        public IRepository<Categoria> categoriaRepository => _categoriaRepository ?? new BaseRepository<Categoria>(_context);

        public IRepository<Servicio> servicioRepository => _servicioRepository ?? new BaseRepository<Servicio>(_context);

        public IRepository<Profesional> profesionalRepository => _profesionalRepository ?? new BaseRepository<Profesional>(_context);

        public IRepository<FormaPago> formaPagoRepository => _formaPagoRepository ?? new BaseRepository<FormaPago>(_context);

        public IRepository<ModalidadPago> modalidadPagoRepository => _modalidadPagoRepository ?? new BaseRepository<ModalidadPago>(_context);

        public IRepository<Paquete> paqueteRepository => _paqueteRepository ?? new BaseRepository<Paquete>(_context);

        public IRepository<Novedades> NewnessRepository => _newnessRepository ?? new BaseRepository<Novedades>(_context);

        public IRepository<PreguntasFrecuentes> FrequentQuestionRepository => _frequentRepository ?? new BaseRepository<PreguntasFrecuentes>(_context);

        public IRepository<Testimonios> TestimonialRepository => _testimonailRepository ?? new BaseRepository<Testimonios>(_context);

        public IRepository<MediosContactoEmpresa> mediosContactoEmpresaRepository => _mediosContactoEmpresaRepository ?? new BaseRepository<MediosContactoEmpresa>(_context);

        public IRepository<Banner> bannerRepository => _bannerRepository ?? new BaseRepository<Banner>(_context);

        public IRepository<HorarioAtencionProfesional> horarioAtencionProfesionalRepository => _horarioAtencionProfesionalRepository ?? new BaseRepository<HorarioAtencionProfesional>(_context);

        public IRepository<MensajeMasivo> mensajeMasivoRepository => _mensajeMasivoRepository ?? new BaseRepository<MensajeMasivo>(_context);

        public IRepository<ServicioPaquete> servicioPaqueteRepository => _servicioPaqueteRepository ?? new BaseRepository<ServicioPaquete>(_context);

        public IRepository<ServicioProfesional> servicioProfesionalRepository => _servicioProfesionalRepository ?? new BaseRepository<ServicioProfesional>(_context);

        public IRepository<OrdenItem> ordenItemsRepository => _ordenItemsRepository ?? new BaseRepository<OrdenItem>(_context);

        public IRepository<Orden> ordenRepository => _ordenRepository ?? new BaseRepository<Orden>(_context);
        public IRepository<CredencialMercadoPago> credencialMercadoPagoRepository => _credencialMercadoPagoRepository ?? new BaseRepository<CredencialMercadoPago>(_context);
        public IRepository<HorarioBloqueado> horarioBloqueadoRepository => _horarioBloqueadoRepository ?? new BaseRepository<HorarioBloqueado>(_context);

        public IRepository<HistoricoIngresos> historicoIngresosRepository => _historicoIngresosRepository ?? new BaseRepository<HistoricoIngresos>(_context);

        public IStoredProcedureRepository reporteProcedureRepository => _reporteProcedureRepository ?? new BaseStoredProcedureRepository(_context);


        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }
        public void SaveChange()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }


    }
}

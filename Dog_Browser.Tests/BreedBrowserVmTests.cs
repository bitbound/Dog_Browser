using Dog_Browser.BaseTypes;
using Dog_Browser.Dtos;
using Dog_Browser.Models;
using Dog_Browser.SampleData;
using Dog_Browser.Services;
using Dog_Browser.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Dog_Browser.Tests
{
    // TODO: Test other view models in similar fashion to below.

#pragma warning disable CS8602 // Dereference of a possibly null reference.
    [TestClass]
    public class BreedBrowserVmTests
    {
        private static DogBreed[] _dogBreeds = new[]
            {
                new DogBreed("retriever", "golden"),
                new DogBreed("pitbull")
            };

        private readonly IDispatcherService _dispatcher = new DispatcherServiceStub();

        private Mock<IDogBreedsApi>? _api;
        private Mock<IDialogService>? _dialog;
        private ApiResponseEventArgs<DogBreed[]>? _failedApiEventArgs =
            new(Result.Fail<DogBreed[]>("Network error."), false);

        private ApiResponseEventArgs<DogBreed[]>? _successApiEventArgs =
            new(Result.Ok(_dogBreeds), false);

        private BreedBrowserViewModel? _viewModel;


        [TestInitialize]
        public void Init()
        {
            _api = new Mock<IDogBreedsApi>();
            _dialog = new Mock<IDialogService>();
        }

        [TestMethod]
        public void ReceivedAllBreeds_GivenFailureResponse_ShowsDialog()
        {
            _viewModel = new BreedBrowserViewModel(_api.Object, _dialog.Object, _dispatcher);

            _api.Raise(x => x.ReceivedAllBreeds += null, _failedApiEventArgs);

            _dialog.Verify(x => x.Show(
                "An error occurred while contacting the Dog Breeds server.  Please check the logs or try again later.",
                "Communication Failure",
                MessageBoxButton.OK,
                MessageBoxImage.Error));

            _api.VerifyAdd(x => x.ReceivedAllBreeds += It.IsAny<EventHandler<ApiResponseEventArgs<DogBreed[]>>>());
            _api.Verify(x => x.GetAllBreeds(), Times.Once);
            _api.VerifyNoOtherCalls();
            _dialog.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void ReceivedAllBreeds_GivenSuccessfulResponse_PopulatesList()
        {

            _viewModel = new BreedBrowserViewModel(_api.Object, _dialog.Object, _dispatcher);

            _api.Raise(x => x.ReceivedAllBreeds += null, _successApiEventArgs);

            Assert.AreEqual(2, _viewModel.DogBreeds.Count);
            Assert.AreEqual("Golden Retriever", _viewModel.DogBreeds[0].DisplayName);
            Assert.AreEqual("Pitbull", _viewModel.DogBreeds[1].DisplayName);

            _api.VerifyAdd(x => x.ReceivedAllBreeds += It.IsAny<EventHandler<ApiResponseEventArgs<DogBreed[]>>>());
            _api.Verify(x => x.GetAllBreeds(), Times.Once);
            _api.VerifyNoOtherCalls();
            _dialog.VerifyNoOtherCalls();
        }

        [TestMethod]
        public void ViewModelConstructed_GivenHappyPath_CallsGetAllBreeds()
        {
            _viewModel = new BreedBrowserViewModel(_api.Object, _dialog.Object, _dispatcher);

            _api.VerifyAdd(x => x.ReceivedAllBreeds += It.IsAny<EventHandler<ApiResponseEventArgs<DogBreed[]>>>());
            _api.Verify(x => x.GetAllBreeds(), Times.Once);
            _api.VerifyNoOtherCalls();
            _dialog.VerifyNoOtherCalls();
        }
    }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
}
